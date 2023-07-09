using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.HistoryBot.App.Model
{
    /// <summary>
    ///     Ученик
    /// </summary>
    public class Student
    {
        /// <summary>
        ///     Логин в телеграме
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Имя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Дата истечения подписки
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        ///     Доступные темы
        /// </summary>
        public string AvailableThemes { get; set; }

        /// <summary>
        ///     Последнее прохождение теста
        /// </summary>
        public Passage LastUsage { get; set; }

        public bool IsTopicAvailable(int topicNumber)
        {
            return true;
        }
    }
}
