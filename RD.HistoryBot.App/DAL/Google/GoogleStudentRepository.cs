using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL.Google
{
    /// <inheritdoc cref="IStudentRepository" />
    internal class GoogleStudentRepository : IStudentRepository
    {
        private static readonly TimeSpan LoadInterval = TimeSpan.FromMinutes(10);

        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId;

        private List<Student> _students;
        private DateTime _lastLoadDate = DateTime.MinValue;

        public GoogleStudentRepository(SheetsService sheetsService, string spreadsheetId)
        {
            _sheetsService = sheetsService;
            _spreadsheetId = spreadsheetId;
            _students = new List<Student>(0);
        }

        public async Task Reload()
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, "Students!A2:B");
            var response = await request.ExecuteAsync();
            var values = response.Values;

            var students = new List<Student>(values.Count);
            foreach (var item in values)
                students.Add(CreateStudent(item));

            _lastLoadDate = DateTime.UtcNow;
            _students = students;
        }

        private Student CreateStudent(IList<object> rowItem)
            => new Student()
            {
                Login = rowItem[0]?.ToString() ?? string.Empty,
                UserName = rowItem[1]?.ToString() ?? string.Empty,
                ExpirationDate = rowItem[2] is DateTime expirationDate ? expirationDate : DateTime.Parse(rowItem[2]?.ToString()),
                AvailableThemes = rowItem[3]?.ToString() ?? string.Empty,
            };

        public async Task<Student?> GetByLogin(string login)
        {
            var student = _students.Find(x => x.Login == login);
            if (student == null && DateTime.UtcNow - _lastLoadDate > LoadInterval)
            {
                await Reload();
                student = _students.Find(x => x.Login == login);
            }
            return student;
        }

        public void SavePassage(Student student, Passage passage)
        {
            throw new NotImplementedException();
        }
    }
}
