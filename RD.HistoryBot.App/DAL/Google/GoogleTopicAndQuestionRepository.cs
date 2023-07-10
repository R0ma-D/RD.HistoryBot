using Google.Apis.Sheets.v4;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL.Google
{
    internal class GoogleTopicAndQuestionRepository : ITopicRepository, IQuestionRepository
    {
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId;

        private List<Topic>? _topics;
        private Dictionary<int, List<Question>>? _questionMap;

        public GoogleTopicAndQuestionRepository(SheetsService sheetsService, string spreadsheetId) 
        {
            _sheetsService = sheetsService;
            _spreadsheetId = spreadsheetId;
        }

        public async Task Reload() 
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, "Topics!A2:B");
            var response = await request.ExecuteAsync();
            var values = response.Values;

            var topics = new List<Topic>(values.Count);
            foreach (var item in values)
                topics.Add(CreateTopic(item));

            var questionMap = new Dictionary<int, List<Question>>(topics.Count);
            foreach (var topic in topics)
            {
                var qRequest = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, $"{topic.Number}!A2:M");
                var qResponse = await qRequest.ExecuteAsync();
                var qValues = qResponse.Values;

                var questions = new List<Question>(qValues.Count);
                foreach (var item in qValues)
                {
                    questions.Add(CreateQuestion(topic, item));
                }
                questionMap[topic.Number] = questions;
            }

            _topics = topics;
            _questionMap = questionMap;
        }

        public List<Question> GetQuestions(int topic) 
            => _questionMap?.TryGetValue(topic, out var questions) == true ? questions : new List<Question>(0);

        public Topic? GetTopic(int id) => _topics?.FirstOrDefault(t => t.Number == id);

        public IEnumerable<Topic> GetTopics() => _topics?.ToList() ?? new List<Topic>(0);

        private static Topic CreateTopic(IList<object> rowItem) 
            => new Topic()
            {
                Number = rowItem[0] is int number ? number : int.Parse(rowItem[0]?.ToString() ?? "0"),
                Title = rowItem[1]?.ToString() ?? string.Empty,
            };

        private static Question CreateQuestion(Topic topic, IList<object> rowItem)
            => new Question()
            {
                TopicNumber = topic.Number,
                QuestionNumber = rowItem[0] is int number ? number : int.Parse(rowItem[0]?.ToString() ?? "0"),
                Header = rowItem[1]?.ToString() ?? string.Empty,
                //TODO
            };
    }
}
