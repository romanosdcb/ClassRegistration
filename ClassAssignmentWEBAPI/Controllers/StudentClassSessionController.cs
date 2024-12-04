using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassSessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentClassSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClassSession>>> GetStudentClassSessions()
        {
            return await _context.StudentClassSessions.ToListAsync();
        }

        [HttpGet("{id}/{FetchType}/{Pram1}/{Pram2}")]
        public async Task<ActionResult<IEnumerable<StudentClassSession>>> GetStudentClassSessionsForInstructor(int id, string FetchType, string Pram1, string Pram2)
        {
            switch (FetchType)
            {
                case "StudentList":
                    return await _context.StudentClassSessions.Where(x => x.ClassSessionID == id).ToListAsync();
                default:
                    return await _context.StudentClassSessions.Where(x => x.ClassSessionID == id).ToListAsync();
            } 
        }

        [HttpGet("{StudentID}/{ListType}")]
        public async Task<ActionResult<IEnumerable<WorkingDailySchedule>>> GetStudentSchedule(int StudentID, string ListType)
        {
            List<WorkingDailySchedule> EmptyList = new List<WorkingDailySchedule>();
            EmptyList.Clear();

            if (ListType == "CourseSchedule")
            {
                List<WorkingDailySchedule> SDS = new List<WorkingDailySchedule>();
                SDS.Clear();

                var SQL1 = "EXEC CheckStudentCourseSchedule " + StudentID.ToString();
                var DBList = await _context.WorkingDailySchedules.FromSqlRaw(SQL1).ToListAsync();
                return DBList;
            }
            else
            {
                return EmptyList;
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentClassSession>> GetStudentClassSession(int id)
        {
            var studentClassSession = await _context.StudentClassSessions.FindAsync(id);

            if (studentClassSession == null)
            {
                return NotFound();
            }

            return studentClassSession;
        }

        [HttpPost]
        public async Task<ActionResult<StudentClassSession>> PostStudentClassSession(StudentClassSession studentClassSession)
        {
            studentClassSession.StudentClassSessionID = 0;
            var resourceUrl = Url.Action(nameof(GetStudentClassSessions), new { CourseID = studentClassSession.StudentClassSessionID });

            if (resourceUrl != null)
            {
                studentClassSession.ClassSessionID = studentClassSession.ClassSessionID;
                studentClassSession.StudentID = studentClassSession.StudentID;
                studentClassSession.TimesAbsent = studentClassSession.TimesAbsent;

                if (studentClassSession.TimesAbsent > 0)
                {
                    studentClassSession.TimesAbsent = studentClassSession.TimesAbsent;
                }
                else
                {
                    studentClassSession.TimesAbsent = null;
                }
            }
            else
            {
                studentClassSession.ClassSessionID = 0;
                studentClassSession.StudentID = 0;
                studentClassSession.TimesAbsent = null;
            }

            _context.StudentClassSessions.Add(studentClassSession);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, studentClassSession);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentClassSession(int id, [FromBody] StudentClassSession studentClassSession)
        {
            if (id != studentClassSession.StudentClassSessionID)
            {
                return BadRequest();
            }

            _context.Entry(studentClassSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassSessionExists(id))
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
        public async Task<ActionResult> DeleteStudentClassSession(int id)
        {
            var studentClassSession = await _context.StudentClassSessions.FindAsync(id);

            if (studentClassSession == null)
            {
                return NotFound();
            }

            _context.StudentClassSessions.Remove(studentClassSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentClassSessionExists(int id)
        {
            return _context.StudentClassSessions.Any(e => e.StudentClassSessionID == id);
        }

        [HttpGet("{CourseID}/{StudentID}/{ActionID}")]
        public async Task<ActionResult<IEnumerable<CourseOfferings>>>
            GetCourseOfferings(int CourseID, int StudentID, string ActionID)
        {
            var SQL = "EXEC ListCourseOffering " + CourseID.ToString() + ", " + StudentID.ToString();
            var DBList = await _context.CourseOfferings.FromSqlRaw(SQL).ToListAsync();
            return DBList;
        }
    }
}
