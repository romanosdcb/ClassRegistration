using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
        {
            //return await _context.Instructors.ToListAsync();


            var ReturnValues = await _context.Instructors.ToListAsync();

            return ReturnValues;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;
        }

        [HttpPost]
        public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
        {
            instructor.InstructorID = 0;
            var resourceUrl = Url.Action(nameof(GetInstructors), new { InstructorID = instructor.InstructorID });

            if (resourceUrl != null)
            {
                instructor.FirstName = instructor.FirstName;

                if (instructor.MiddleName == null || instructor.MiddleName.Length == 0)
                {
                    instructor.MiddleName = ".";
                }
                else
                {
                instructor.MiddleName = instructor.MiddleName;
                }

                instructor.LastName = instructor.LastName;
                instructor.Title = instructor.Title;
                instructor.PhoneNumber = instructor.PhoneNumber;
                instructor.AddressLine1 = instructor.AddressLine1;
                instructor.AddressLine2 = instructor.AddressLine2;
                instructor.City = instructor.City;
                instructor.State = instructor.State;
                instructor.ZipCode = instructor.ZipCode;
            }
            else
            {
                instructor.FirstName = string.Empty;
                instructor.MiddleName = string.Empty;
                instructor.LastName = string.Empty;
                instructor.Title = string.Empty;
                instructor.PhoneNumber = string.Empty;
                instructor.AddressLine1 = string.Empty;
                instructor.AddressLine2 = string.Empty;
                instructor.City = string.Empty;
                instructor.State = string.Empty;
                instructor.ZipCode = string.Empty;
            }

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, instructor);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, [FromBody] Instructor instructor)
        {
            if (id != instructor.InstructorID)
            {
                return BadRequest();
            }

            if (instructor.MiddleName == null || instructor.MiddleName.Length == 0)
            {
                instructor.MiddleName = ".";
            }

            _context.Entry(instructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(id))
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
        public async Task<ActionResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorID == id);
        }
    }
}
