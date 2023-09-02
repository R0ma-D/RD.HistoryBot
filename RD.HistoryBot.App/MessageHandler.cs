using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.DAL;
using RD.HistoryBot.App.Model;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;

namespace RD.HistoryBot.App
{
    internal class MessageHandler
    {
        private const string TELEGRAM_PREFIX = "t.me/";
        private static readonly TimeSpan TELEGRAM_LICENSE_TIMEOUT = TimeSpan.FromHours(1);

        private readonly IMemoryCache _memoryCache;
        private readonly IStudentRepository _studentRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;

        public MessageHandler(IStudentRepository studentRepository,
                              ITopicRepository topicRepository,
                              IQuestionRepository questionRepository, 
                              IMemoryCache memoryCache)
        {
            _studentRepository = studentRepository;
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _memoryCache = memoryCache;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message || update.Type == UpdateType.CallbackQuery)
            {
                var message = update.Type == UpdateType.CallbackQuery 
                    ? update.CallbackQuery.Message 
                    : update.Message;
                var text = message?.Text?.ToLower();

                switch (text)
                {
                    case "/start":
                        await Start(botClient, message!);
                        return;
                }

                var student = await GetStudent(botClient, message);
                if (student == null)
                    return;

                var stateKey = $"state_{message.Chat.Id}";
                var state = _memoryCache.Get<State>(stateKey);
                if (state == null)
                {
                    if (int.TryParse(text, out int topicNumber))
                    {
                        var topic = _topicRepository.GetTopic(topicNumber);
                        if (topic != null && student.IsTopicAvailable(topicNumber))
                            state = new State { Topic = topic };
                    }

                    if (state == null)
                    {
                        await Messages.SelectTheme(botClient, message.Chat.Id, student.AvailableThemes);
                        return;
                    }

                    _memoryCache.Set(stateKey, state);
                }

                Question? nextQuestion;
                var currentQuestion = state.Question;
                if (currentQuestion == null) //Самый первый вопрос в теме
                {
                    await Messages.StartTheme(botClient, message.Chat.Id, state.Topic.Title);
                    nextQuestion = GetNextQuestion(state, true);
                }
                else
                {
                    var isCorrectAnswer = update.Type == UpdateType.CallbackQuery
                        ? update.CallbackQuery!.Data == "0" //Правильный ответ тот, которому проставлен 0
                        : currentQuestion.IsCorrectAnswer(text);

                    if (isCorrectAnswer)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, currentQuestion.MessageAfterCorrectAnswer);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, currentQuestion.MessageAfterIncorrectAnswer);
                    }
                    //TODO Calculate Score
                    nextQuestion = GetNextQuestion(state, isCorrectAnswer);
                }

                if (nextQuestion == null)
                {
                    _memoryCache.Remove(stateKey);
                    await Messages.EndTheme(botClient, message.Chat.Id, state.Topic.Title);
                    return;
                }

                state.Question = nextQuestion;
                await SendQuestion(botClient, message.Chat.Id, nextQuestion);
                return;
            }
        }

        private Question? GetNextQuestion(State state, bool isCorrectAnswer)
        {
            var currentQuestionNumber = state.Question?.QuestionNumber ?? 0;
            if (!isCorrectAnswer)
            {
                state.UsedAlternatives.Add(state.Question!.AlternativeOption);
                var alternativeQuestion = _questionRepository.GetQuestions(state.Topic.Number)
                                    .Where(q => q.QuestionNumber == currentQuestionNumber)
                                    .Where(q => !state.UsedAlternatives.Contains(q.AlternativeOption))
                                    .OrderBy(_ => Random.Shared.Next(100))
                .FirstOrDefault();

                if (alternativeQuestion != null)
                    return alternativeQuestion;
            }

            state.UsedAlternatives.Clear();
            var nextQuestions = _questionRepository.GetQuestions(state.Topic.Number)
                                    .Where(q => q.QuestionNumber > currentQuestionNumber)
                                    .OrderBy(q => q.QuestionNumber).ToList();
            if (nextQuestions.Count == 0)
                return null;

            var nextQuestionNumber = nextQuestions[0].QuestionNumber;
            return nextQuestions.Where(q => q.QuestionNumber == nextQuestionNumber)
                                .OrderBy(_ => Random.Shared.Next(100)).First();
        }

        private async Task SendQuestion(ITelegramBotClient botClient, long chatId, Question question)
        {
            var message = $"<strong>{question.QuestionNumber}. {question.Header}</strong>";
            if (!string.IsNullOrEmpty(question.Body))
                message += $"\r\n{question.Body}";
            if (!string.IsNullOrEmpty(question.Footer)) 
                message += $"\r\n<em>{question.Footer}</em>";

            await botClient.SendTextMessageAsync(chatId, message, parseMode: ParseMode.Html);
            if (!string.IsNullOrEmpty(question.Image) && Uri.TryCreate(question.Image, UriKind.Absolute, out _))
                await botClient.SendPhotoAsync(chatId, new InputFileUrl(question.Image));

            if (question.PossibleAnswers?.Any() == true)
            {
                var answers = new[] { question.CorrectAnswer }.Union(question.PossibleAnswers)
                                      .Select((a, i) => new { Text = a, OriginalIndex = i })
                                      .OrderBy(_ => Random.Shared.Next(100))
                                      .ToArray();

                var buttons = new InlineKeyboardButton[answers.Length];
                var letters = "АБВГДЕЖЗИКЛМНОПРСТ";
                for (int i = 0; i < answers.Length && i < letters.Length; i++)
                {
                    var data = answers[i];
                    buttons[i] = new InlineKeyboardButton($"{letters[i]}. {data.Text}") { CallbackData = data.OriginalIndex.ToString() };
                }
                await botClient.SendTextMessageAsync(chatId, 
                    "<em>Выберите 1 вариант:</em>", 
                    parseMode: ParseMode.Html, 
                    //Каждый ответ с новой строки
                    replyMarkup: new InlineKeyboardMarkup(buttons.Select(b => new[] { b })));
            }
        }

        private async Task Start(ITelegramBotClient botClient, Message message)
        {
            var student = await GetStudent(botClient, message);
            if (student == null)
                return;

            await Messages.Greating(botClient, message.Chat.Id, student.UserName);
        }

        private async Task<Student?> GetStudent(ITelegramBotClient botClient, Message message)
        {
            var studentKey = $"student_{message.Chat.Id}";
            if (_memoryCache.TryGetValue<Student>(studentKey, out var student) && student!.ExpirationDate > DateTime.UtcNow)
                return student;

            var userName = GetUserName(message);
            if (string.IsNullOrEmpty(userName))
            {
                await Messages.AccountNotDefined(botClient, message.Chat.Id);
                return null;
            }

            student = await _studentRepository.GetByLogin(userName);
            if (student == null)
            {
                await Messages.AccountNotFound(botClient, message.Chat.Id, userName);
                return null;
            }

            if (student.ExpirationDate < DateTime.UtcNow)
            {
                await Messages.AccountExpired(botClient, message.Chat.Id, userName);
                return null;
            }

            _memoryCache.Set<Student>(studentKey, student, TELEGRAM_LICENSE_TIMEOUT);
            return student;
        }

        private static string GetUserName(Message userMessage)
        {
            var userName = userMessage.From.Username ?? string.Empty;
            if (userName.StartsWith(TELEGRAM_PREFIX, StringComparison.OrdinalIgnoreCase))
                userName = userName.Substring(TELEGRAM_PREFIX.Length);

            return userName;
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //Log(exception);
        }
    }
}
