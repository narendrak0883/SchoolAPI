using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[Route("api/class")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
    {
        return Ok(await _classService.GetClasses());
    }

    [HttpPost]
    public async Task<ActionResult<Class>> PostClass(Class classType)
    {
        var createdClass = await _classService.AddClass(classType);

        return CreatedAtAction(nameof(GetClasses), new { id = createdClass.Id }, createdClass);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutClass(int id, Class classType)
    {
        if (id != classType.Id)
        {
            return BadRequest();
        }

        try
        {
            await _classService.UpdateClass(id, classType);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_classService.ClassExists(id))
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
    public async Task<IActionResult> DeleteClass(int id)
    {
        if (!_classService.ClassExists(id))
        {
            return NotFound();
        }

        await _classService.DeleteClass(id);
        return NoContent();
    }
}
