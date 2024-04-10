using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Services
{
    public class ClassService : IClassService
    {
        private readonly SchoolContext _context;

        public ClassService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class> AddClass(Class c)
        {
            _context.Classes.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<Class> UpdateClass(int id, Class c)
        {
            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task DeleteClass(int id)
        {
            var classType = await _context.Classes.FindAsync(id);
            _context.Classes.Remove(classType);
            await _context.SaveChangesAsync();
        }

        public bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
