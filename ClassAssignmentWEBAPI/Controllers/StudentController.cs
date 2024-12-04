using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            student.StudentID = 0;
            var resourceUrl = Url.Action(nameof(GetStudents), new { StudentID = student.StudentID });

            if (resourceUrl != null)
            {
                student.FirstName = student.FirstName;

                if (student.MiddleName == null || student.MiddleName.Length == 0)
                {
                    student.MiddleName = ".";
                }
                else
                {
                    student.MiddleName = student.MiddleName;
                }

                student.LastName = student.LastName;
                student.ClassRank = student.ClassRank;
                student.PhoneNumber = student.PhoneNumber;
                student.AddressLine1 = student.AddressLine1;
                student.AddressLine2 = student.AddressLine2;
                student.City = student.City;
                student.State = student.State;
                student.ZipCode = student.ZipCode;
            }
            else
            {
                student.FirstName = string.Empty;
                student.MiddleName = string.Empty;
                student.LastName = string.Empty;
                student.ClassRank = null;
                student.PhoneNumber = string.Empty;
                student.AddressLine1 = string.Empty;
                student.AddressLine2 = string.Empty;
                student.City = string.Empty;
                student.State = string.Empty;
                student.ZipCode = string.Empty;
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, student);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            if (student.MiddleName == null || student.MiddleName.Length == 0)
            {
                student.MiddleName = ".";
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
