using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL.InMemory
{
    /// <inheritdoc cref="ITopicRepository" />
    internal class InMemoryTopicRepository : ITopicRepository
    {
        private readonly List<Topic> _topics = new List<Topic>() 
        { 
            new Topic() { Number = 1, Title = "Тема 1" },
            new Topic() { Number = 2, Title = "Тема 2" },
            new Topic() { Number = 3, Title = "Тема 3" },
            new Topic() { Number = 4, Title = "Тема 4" },
            new Topic() { Number = 5, Title = "Тема 5" },
            new Topic() { Number = 6, Title = "Тема 6" },
        };

        public Topic GetTopic(int id)
        {
            return _topics.FirstOrDefault(t => t.Number == id);
        }

        public IEnumerable<Topic> GetTopics()
        {
            return _topics.AsReadOnly();
        }
    }
}
