using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return Ok(await _teacherService.GetTeachers());
        }

        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            var createdTeacher = await _teacherService.AddTeacher(teacher);
            return CreatedAtAction(nameof(GetTeachers), new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
                return BadRequest();

            try
            {
                await _teacherService.UpdateTeacher(id, teacher);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_teacherService.TeacherExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            if (!_teacherService.TeacherExists(id))
                return NotFound();

            await _teacherService.DeleteTeacher(id);

            return NoContent();
        }
    }

}
