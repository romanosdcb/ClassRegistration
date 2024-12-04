using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            course.CourseID = 0;
            var resourceUrl = Url.Action(nameof(GetCourses), new { CourseID = course.CourseID });

            if (resourceUrl != null)
            {
                course.Title = course.Title;
                course.Department = course.Department;
                course.CreditHours = course.CreditHours;

                if (course.PrerequisiteCourseID > 0)
                {
                    course.PrerequisiteCourseID = course.PrerequisiteCourseID;
                }
                else
                {
                    course.PrerequisiteCourseID = null;
                }
            }
            else
            {
                course.Title = "";
                course.Department = "";
                course.CreditHours = 0;
                course.PrerequisiteCourseID = null;
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, course);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, [FromBody] Course course)
        {
            if (id != course.CourseID)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
