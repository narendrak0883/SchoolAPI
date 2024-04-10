using SchoolAPI.Models;

namespace SchoolAPI.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetStudents();
    Task<Student> AddStudent(Student student);
    Task<Student> UpdateStudent(int id, Student student);
    Task DeleteStudent(int id);
    bool StudentExists(int id);
}