using SchoolAPI.Models;

namespace SchoolAPI.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetTeachers();
        Task<Teacher> AddTeacher(Teacher teacher);
        Task<Teacher> UpdateTeacher(int id, Teacher teacher);
        Task DeleteTeacher(int id);
        bool TeacherExists(int id);
    }
}
