using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return Ok(await _studentService.GetStudents());
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var createdStudent = await _studentService.AddStudent(student);
            return CreatedAtAction(nameof(GetStudents), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            try
            {
                await _studentService.UpdateStudent(id, student);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_studentService.StudentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (!_studentService.StudentExists(id))
                return NotFound();

            await _studentService.DeleteStudent(id);

            return NoContent();
        }
    }


}
