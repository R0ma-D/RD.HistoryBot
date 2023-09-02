using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL.InMemory
{
    internal class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;

        public InMemoryStudentRepository(IEnumerable<Student> students) 
        {
            _students = students.ToList();
        }

        public Task<Student?> GetByLogin(string login)
        {
            return Task.FromResult(_students.FirstOrDefault(x => x.Login == login));
        }

        public void SavePassage(Student student, Passage passage)
        {
            //Do nothing for temp
        }
    }
}
