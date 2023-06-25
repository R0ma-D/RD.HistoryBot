namespace RD.HistoryBot.App.Model
{
    /// <summary>
    ///     Результат ответа на вопрос
    /// </summary>
    public class PassageQuestionResult
    {
        /// <summary>
        ///     Текст вопроса
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        ///     Ответ студента
        /// </summary>
        public string StudentAnswer { get; set; }

        /// <summary>
        ///     Засчитан ли ответ за правильный
        /// </summary>
        public bool IsCorrect { get; set; }
    }
}
