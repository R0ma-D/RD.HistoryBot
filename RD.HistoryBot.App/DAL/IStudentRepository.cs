using RD.HistoryBot.App.Model;

namespace RD.HistoryBot.App.DAL
{
    /// <summary>
    ///     Хранилище списка учеников
    /// </summary>
    public interface IStudentRepository
    {
        /// <summary>
        ///     Получить данные об ученике по его логину в телеграме 
        /// </summary>
        /// <param name="login">Логин ученика</param>
        /// <returns>Возвращает данные об ученике</returns>
        Student? GetByLogin(string login);

        /// <summary>
        ///     Сохранить попытку прохождения темы
        /// </summary>
        /// <param name="student">Студент, который сдавал тему</param>
        /// <param name="passage">Результат сдачи</param>
        void SavePassage(Student student, Passage passage);
    }
}
