using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolContext _context;

        public SchoolService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<School>> GetSchools()
        {
            return await _context.Schools.ToListAsync();
        }

        public async Task<School> GetSchool(int id)
        {
            return await _context.Schools.FindAsync(id);
        }

        public async Task<School> AddSchool(School school)
        {
            _context.Schools.Add(school);
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task<School> UpdateSchool(int id, School school)
        {
            _context.Entry(school).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task DeleteSchool(int id)
        {
            var school = await _context.Schools.FindAsync(id);

            if (school != null)
            {
                _context.Schools.Remove(school);
                await _context.SaveChangesAsync();
            }
        }

        public bool SchoolExists(int id)
        {
            return _context.Schools.Any(e => e.Id == id);
        }
    }
}
