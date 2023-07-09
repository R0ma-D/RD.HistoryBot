using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL
{
    /// <summary>
    ///     Хранилище списка тем
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        ///     Получить список тем
        /// </summary>
        /// <returns>Список тем</returns>
        IEnumerable<Topic> GetTopics();

        /// <summary>
        ///     Получить тему по её номеру
        /// </summary>
        /// <param name="id">Номер темы</param>
        /// <returns>Тему</returns>
        Topic GetTopic(int id);
    }
}
