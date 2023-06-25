using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL.InMemory
{
    internal class InMemoryQuestionRepository : IQuestionRepository
    {
        private static readonly List<Question> _questions = new List<Question>()
        {
            new Question() {
                TopicNumber = 1,
                QuestionNumber = 1,
                AlternativeOption = "1",
                Header = "Первый металл, который в древности на белорусских землях начали добывать люди - это:",
                CorrectAnswer = "железо",
                PossibleAnswers = new List<string>{ "золото", "медь", "олово", "серебро" },
                MessageAfterCorrectAnswer = "Поздравляю! Это действительно железо.",
                MessageAfterIncorrectAnswer = "Эх, неверно. Золота и серебра на территории Беларуси нет. Медь и олово находятся слишком глубоко, чтобы их можно было добыть. Поэтому в бронзовом веке использовали только привезённую бронзу. Правильный ответ: железо",
            },
            new Question() {
                TopicNumber = 1,
                QuestionNumber = 1,
                AlternativeOption = "2",
                Header = "Определите металл, который люди начала использовать на белорусских землях во 2-м тыс. до н.э.:",
                CorrectAnswer = "медь",
                PossibleAnswers = new List<string>{ "золото", "сталь", "железо", "серебро" },
                MessageAfterCorrectAnswer = "Поздравляю! Это действительно медь.",
                MessageAfterIncorrectAnswer = "Эх, неверно. Обрати внимание, в вопросе написано 'использовать', первым металлом, который начали использовать на территории Беларуси была: медь.",
            },
            new Question() {
                TopicNumber = 1,
                QuestionNumber = 2,
                AlternativeOption = "1",
                Header = "Совершенствование земледелия и орудий труда в бронзовом и раннем железном веках привёл к:",
                CorrectAnswer = "появлению излишков продуктов",
                PossibleAnswers = new List<string>{ "возникновению родовой общины", "установлению равенства в обществе", "появлению государства", "уравнительному распределению продуктов" },
                MessageAfterCorrectAnswer = "Поздравляю! Это действительно так.",
                MessageAfterIncorrectAnswer = "В следующий раз обязательно получится. Правильный ответ был: появлению излишков продуктов",
            },

            new Question() {
                TopicNumber = 2,
                QuestionNumber = 1,
                Header = "Для содействия развития торговли в 1766 г. в Речи Посполитой были введены единые:",
                CorrectAnswer = "меры длины, веса и объёма",
                PossibleAnswers = new List<string>{ "правила торговли", "цены на сырьё", "крестьянские повинности", "налоги" },
                MessageAfterCorrectAnswer = "Поздравляю! Это действительно меры длины, веса и объёма.",
                MessageAfterIncorrectAnswer = "Эх, неверно. Правильно: меры длины, веса и объёма.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 2,
                Header = "Определите характерные черты политики \"военного коммуниза\":",
                IsManual = true,
                CorrectAnswer = "3,4,5",
                PossibleAnswers = new List<string>{ "введение продналога", "введение в оборот советского червонца", "запрещение свободной торговли", "национализация промышленности", "введение всеобщей трудовой повинности" },
                MessageAfterCorrectAnswer = "Поздравляю! Это всё верно.",
                MessageAfterIncorrectAnswer = "Это был непростой вопрос. Правильно сразу три пункта: 'запрещение свободной торговли', 'национализация промышленности' и 'введение всеобщей трудовой повинности'.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 3,
                Header = "Установите соответствие:",
                Body = "А) принятие I Статута ВКЛ\r\nБ) перенос столицы ВКЛ в Вильно\r\nВ) заключение Островского соглашения между Витовтом и Ягайло\r\nГ) Начало гражданской войны в ВКЛ между Свидригайло и Сигезмундом Кейстутовичем\r\n"
                    + "1) 1323\r\n2) 1392\r\n3) 1432\r\n4) 1529\r\n5) 1566",
                Footer = "Ответ запишите в формате: А1, Б2, и т.д.",
                IsManual = true,
                CorrectAnswer = "А1, Б2, В3, Г5",
                MessageAfterCorrectAnswer = "Поздравляю! Это было восхительно.",
                MessageAfterIncorrectAnswer = "Эх, неверно.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 4,
                Header = "Прочитайте отрывок из книги М. Кояловича \"Чтения по истории Западной России\":",
                Body = "\"... Настоит вопиющая ... не только Белоруссию, но и Украину и Литву\".",
                Footer = "Определите два правильных утверждения (ответ запишите в формате '1, 3'):",
                IsManual = true,
                CorrectAnswer = "1, 5",
                PossibleAnswers = new List<string>(){ "автор выступает за русификацию Беларуси, Украины и Литвы", "автор признаёт существование белорусской нации", "учёный поддерживает мнение, что Беларусь, Украина и Литва - часть Польши", "М. Коялович выступает за право наций на самоопределение", "система взглядов, изложенная в тексте, получила название \"западноруссизм\"" },
                MessageAfterCorrectAnswer = "Поздравляю! Это было восхительно.",
                MessageAfterIncorrectAnswer = "Эх, неверно.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 5,
                Header = "Вставьте в приведённом тексте на места пропусков подходащие по смыслу слова из предложенного списка:",
                Body = "Впервые в летописи Туров упоминается в А)____ году. Долгое время Туров зависел от Б)____. Обрёл самостоятельность только в 1157 г. при князе В)____.\r\n"
                    + "1) 862\r\n2) 980\r\n3) Полоцк\r\n4) Киев\r\n5) Юрий Ярославович\r\n6) Изяслав Ярославович",
                Footer = "Ответ запишите в формате: А1, Б2, и т.д.",
                IsManual = true,
                CorrectAnswer = "А1, Б3, В5",
                MessageAfterCorrectAnswer = "Поздравляю! Это было восхительно.",
                MessageAfterIncorrectAnswer = "Эх, неверно.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 6,
                Header = "В Древней Руси отряд вооружённых, специально подготовленных к военному делу людей, советников князя, назывался ____.",
                IsManual = true,
                CorrectAnswer = "Дружина",
                MessageAfterCorrectAnswer = "Поздравляю! Правильно, это дружина.",
                MessageAfterIncorrectAnswer = "Эх, неверно. Правильный ответ: дружина",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 7,
                Header = "При поддержке Стефана Батория виленский иезуитский коллегиум был преобразован в Виленскую академию в ____ году.",
                IsManual = true,
                CorrectAnswer = "1579",
                MessageAfterCorrectAnswer = "Поздравляю! Правильно, это случилось в 1579 году.",
                MessageAfterIncorrectAnswer = "Эх, жаль. На самом деле это случилось в 1579 году.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 8,
                AlternativeOption = "1",
                Header = "Территория, обозначенная на карте цифрой 2, была присоединена к БССР в ____ году.",
                Image = null,
                IsManual = true,
                CorrectAnswer = "1924",
                MessageAfterCorrectAnswer = "Поздравляю! Правильно, это случилось в 1924 году.",
                MessageAfterIncorrectAnswer = "Эх, жаль. На самом деле это случилось в 1924 году.",
            },
            new Question() {
                TopicNumber = 2,
                QuestionNumber = 8,
                AlternativeOption = "2",
                Header = "Территория, обозначенная на карте цифрой 2, была присоединена к БССР в ____ году.",
                Image = null,
                CorrectAnswer = "1924",
                PossibleAnswers = new List<string>() { "1921", "1926", "1939" },
                MessageAfterCorrectAnswer = "Поздравляю! Правильно, это случилось в 1924 году.",
                MessageAfterIncorrectAnswer = "Эх, жаль. На самом деле это случилось в 1924 году.",
            },
        };


        public List<Question> GetQuestions(int topic)
        {
            return _questions.Where(q => q.TopicNumber == topic).OrderBy(q => q.QuestionNumber).ToList();
        }
    }
}
