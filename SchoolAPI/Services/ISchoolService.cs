using SchoolAPI.Models;

namespace SchoolAPI.Services
{
    public interface ISchoolService
    {
        Task<IEnumerable<School>> GetSchools();
        Task<School> GetSchool(int id);
        Task<School> AddSchool(School school);
        Task<School> UpdateSchool(int id, School school);
        Task DeleteSchool(int id);
        bool SchoolExists(int id);
    }
}
