using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RD.HistoryBot.App.DAL;
using RD.HistoryBot.App.DAL.InMemory;
using RD.HistoryBot.App.Model;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RD.HistoryBot.App
{
    internal class Program
    {
        private const string TELEGRAM_PREFIX = "t.me/";

        private static IMemoryCache _memoryCache;
        private static IStudentRepository _studentRepository;
        private static IQuestionRepository _questionRepository;

        static void Main(string[] args)
        {
            Log("Starting history bot...");
            
            try
            {
                _memoryCache = new MemoryCache(new MemoryCacheOptions());
                _questionRepository = new InMemoryQuestionRepository();
                
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets(typeof(Program).GetTypeInfo().Assembly)
                    .AddEnvironmentVariables()
                    .Build();

                var testStudents = config["Bot:Admins"]?
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Select(x => new Student
                    {
                        Login = x,
                        UserName = x,
                        ExpirationDate = DateTime.UtcNow.AddDays(1),
                        AvailableThemes = "1-5",
                    }).ToArray() ?? Array.Empty<Student>();

                _studentRepository = new InMemoryStudentRepository(testStudents);

                var botClient = GetBotClient(config);

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { }, // receive all update types
                };

                botClient.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );

                Log("Bot is ready");

                while (true)
                {
                    var input = Console.ReadLine()?.ToLower();
                    if (input == "exit")
                    {
                        cts.Cancel();
                        return;
                    }
                    Console.WriteLine("To stop bot, type 'exit' and press Enter");
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                throw;
            }
        }

        private static ITelegramBotClient GetBotClient(IConfiguration config)
        {
            var token = config["Bot:Token"];
            return new TelegramBotClient(token!);
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var text = message?.Text?.ToLower();

                switch (text)
                {
                    case "/start":
                        await Start(botClient, message!);
                        return;
                }

            }
            else if (update.Type == UpdateType.CallbackQuery)
            {

            }
        }

        private static Task Start(ITelegramBotClient botClient, Message message)
        {
            var userName = GetUserName(message);
            if (string.IsNullOrEmpty(userName))
                return Messages.AccountNotDefined(botClient, message.Chat.Id);

            var student = _studentRepository.GetByLogin(userName);
            if (student == null)
                return Messages.AccountNotFound(botClient, message.Chat.Id, userName);

            if (student.ExpirationDate < DateTime.UtcNow)
                return Messages.AccountExpired(botClient, message.Chat.Id, userName);

            var userId = message.From.Id;
            return Task.CompletedTask;
        }

        private static string GetUserName(Message userMessage)
        {
            var userName = userMessage.From.Username ?? string.Empty;
            if (userName.StartsWith(TELEGRAM_PREFIX, StringComparison.OrdinalIgnoreCase))
                userName = userName.Substring(TELEGRAM_PREFIX.Length);

            return userName;
        }

        


        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Log(exception);
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static void Log(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}