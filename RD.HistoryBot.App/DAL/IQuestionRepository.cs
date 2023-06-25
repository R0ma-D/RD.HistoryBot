using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL
{
    /// <summary>
    ///     Хранилище списка вопросов
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        ///     Получить список вопросов по теме
        /// </summary>
        /// <param name="topic">Тема</param>
        /// <returns>Список вопросов</returns>
        List<Question> GetQuestions(int topic);
    }
}
