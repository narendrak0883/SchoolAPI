using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
    [Route("api/school")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<School>>> GetSchools()
        {
            return Ok(await _schoolService.GetSchools());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<School>> GetSchool(int id)
        {
            var school = await _schoolService.GetSchool(id);

            if (school == null)
            {
                return NotFound();
            }

            return school;
        }

        [HttpPost]
        public async Task<ActionResult<School>> PostSchool(School school)
        {
            var createdSchool = await _schoolService.AddSchool(school);

            return CreatedAtAction("GetSchool", new { id = createdSchool.Id }, createdSchool);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchool(int id, School school)
        {
            if (id != school.Id)
            {
                return BadRequest();
            }

            try
            {
                await _schoolService.UpdateSchool(id, school);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_schoolService.SchoolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var school = await _schoolService.GetSchool(id);
            if (school == null)
            {
                return NotFound();
            }

            await _schoolService.DeleteSchool(id);

            return NoContent();
        }
    }


}
