using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.HistoryBot.App.Model
{
    /// <summary>
    ///     Попытка прохождения темы
    /// </summary>
    public class Passage
    {
        /// <summary>
        ///     Тема теста
        /// </summary>
        public int Topic { get; set; }

        /// <summary>
        ///     Дата прохождения теста
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Сколько времени потрачено на прохождение (в минутах)
        /// </summary>
        public double DurationInMinutes { get; set; }

        /// <summary>
        ///     Набранное количество баллов
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        ///     Закрытые вопросы (успешно или нет)
        /// </summary>
        public List<PassageQuestionResult> ClosedQuestions { get; set; }
    }
}
