using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.HistoryBot.App.Model
{
    /// <summary>
    ///     Вопрос
    /// </summary>
    public class Question
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public Question() { }

        /// <summary>
        ///     Номер темы
        /// </summary>
        public int TopicNumber { get; set; }

        /// <summary>
        ///     Номер вопроса (в одной теме может быть несколько вопросов с одним номером)
        /// </summary>
        public int QuestionNumber { get; set; }

        /// <summary>
        ///     Вариант альтернативы 
        /// </summary>
        public string AlternativeOption { get; set; }

        /// <summary>
        ///     Заголовок вопроса (может быть совмещён с <see cref="Body"/> )
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        ///     Текст вопроса 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     Описание, как нужно отвечать
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        ///     Картинка
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        ///     True - ответ вводится вручную;
        ///     False - ответ выбирается из списка (для случая, когда подразумевается только 1 правильный вариант)
        /// </summary>
        public bool IsManual { get; set; }

        /// <summary>
        ///     Правильный ответ
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        ///     Варианты ответов
        /// </summary>
        public List<string> PossibleAnswers { get; set; }

        /// <summary>
        ///     Похвала за правильный ответ
        /// </summary>
        public string MessageAfterCorrectAnswer { get; set; }

        /// <summary>
        ///     Слова поддержки и пояснение в случае неправильного ответа
        /// </summary>
        public string MessageAfterIncorrectAnswer { get; set; }
    }
}
