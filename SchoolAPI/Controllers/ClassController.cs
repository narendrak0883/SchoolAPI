using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[Route("api/class")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly SchoolContext _context;

    public ClassController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
    {
        return await _context.Classes.ToListAsync();
    }

    // POST 
    [HttpPost]
    public async Task<ActionResult<Class>> PostClass(Class classType)
    {
        _context.Classes.Add(classType);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClasses), new { id = classType.Id }, classType);
    }

    // PUT 
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClass(int id, Class classType)
    {
        if (id != classType.Id)
            return BadRequest();

        _context.Entry(classType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClassExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var classType = await _context.Classes.FindAsync(id);
        if (classType == null)
            return NotFound();

        _context.Classes.Remove(classType);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClassExists(int id) => _context.Classes.Any(e => e.Id == id);

}