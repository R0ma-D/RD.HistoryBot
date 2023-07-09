using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.HistoryBot.App.Model
{
    /// <summary>
    ///     Состояние чата с пользователем
    /// </summary>
    internal class State
    {
        /// <summary>
        ///     Текущая тема
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        ///     Текущий вопрос темы
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        ///     Альтернативы, которые использовали
        /// </summary>
        public List<string> UsedAlternatives { get; } = new List<string>();
    }
}
