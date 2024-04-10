using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolContext _context;

        public TeacherService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher> AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher> UpdateTeacher(int id, Teacher teacher)
        {
            _context.Entry(teacher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }

        public bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
