using SchoolAPI.Models;

namespace SchoolAPI.Services;

public interface IClassService
{
    Task<IEnumerable<Class>> GetClasses();
    Task<Class> AddClass(Class c);
    Task<Class> UpdateClass(int id, Class c);
    Task DeleteClass(int id);
    bool ClassExists(int id);
}