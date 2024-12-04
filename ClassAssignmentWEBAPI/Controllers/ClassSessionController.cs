using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSession>>> GetClassSessions()
        {
            return await _context.ClassSessions.ToListAsync();
        }

        [HttpGet("{RecordID}/{DOW}/{ActionName}")]
        public async Task<ActionResult<IEnumerable<WorkingDailySchedule>>> GetInstructorSchedules(int RecordID, string DOW, string ActionName)
        {
            List<WorkingDailySchedule> IDS = new List<WorkingDailySchedule>();
            IDS.Clear();

            switch (ActionName)
            {
                case "Instructor":
                    var SQL1 = "EXEC InstructorScheduleByDayOfWeek " + RecordID.ToString() + ", '" + DOW + "'";
                    var SentDBList1 = await _context.WorkingDailySchedules.FromSqlRaw(SQL1).ToListAsync();
                    return SentDBList1;

                case "ClassRoom":
                    var SQL2 = "EXEC ClassRoomScheduleByDayOfWeek " + RecordID.ToString() + ", '" + DOW + "'";
                    var SentDBList2 = await _context.WorkingDailySchedules.FromSqlRaw(SQL2).ToListAsync();
                    return SentDBList2;

                default:
                    return IDS;
            }
        }

        [HttpGet("{RecordID}/{DOW}/{StartTime}/{EndTime}/{ConflictType}")]
        public async Task<ActionResult<IEnumerable<WorkingDailyConflicts>>> GetWorkingDailyConflicts(
            int RecordID, string DOW, TimeOnly StartTime, TimeOnly EndTime, string ConflictType)
        {
            switch (ConflictType)
            {
                case "Instructor":
                    var SQL1 = "EXEC CheckInstructorSchedule " + RecordID.ToString()
                        + ", '" + DOW + "','" + StartTime.ToString() + "', '" + EndTime.ToString() + "'";
                    var InstructorConflictCount = await _context.WorkingDailyConflicts.FromSqlRaw(SQL1).ToListAsync();

                    return InstructorConflictCount;

                case "ClassRoom":
                    var SQL2 = "EXEC CheckClassRoomSchedule " + RecordID.ToString()
                        + ", '" + DOW + "','" + StartTime.ToString() + "', '" + EndTime.ToString() + "'";
                    var ClassRoomConflictCount = await _context.WorkingDailyConflicts.FromSqlRaw(SQL2).ToListAsync();

                    return ClassRoomConflictCount;

                default:
                    List<WorkingDailyConflicts> EmptyList = new List<WorkingDailyConflicts>();
                    EmptyList.Clear();

                    return EmptyList;
            }
        }

        //[HttpGet("{ClassRoomID}/{DOW}/{CR}")]
        //public async Task<ActionResult<IEnumerable<ClassRoomDailySchedule>>> GetClassRoomSchedules(int ClassRoomID, string DOW, int CR)
        //{
        //    var SQL = "EXEC ClassRoomScheduleByDayOfWeek " + ClassRoomID.ToString() + ", '" + DOW + "'";
        //    var SentDBList = await _context.ClassRoomDailySchedules.FromSqlRaw(SQL).ToListAsync();

        //    return SentDBList;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassSession>> GetClassSession(int id)
        {
            var classSession = await _context.ClassSessions.FindAsync(id);

                    if (classSession == null)
            {
                return NotFound();
            }

            return classSession;
        }


       
      


        [HttpPost]
        public async Task<ActionResult<ClassSession>> PostClassSession(ClassSession classSession)
        {
            classSession.ClassSessionID = 0;
            //diaryEntry.Title = "Post URL";

            //_context.DiaryEntries.Add(diaryEntry);
            //await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetClassSessions), new { ClassSessionID = classSession.ClassSessionID });

            if (resourceUrl != null)
            {
                //diaryEntry.Content = resourceUrl;
                //diaryEntry.Title = diaryEntry.Title;
                classSession.WeekDay = classSession.WeekDay;
                classSession.StartTime = classSession.StartTime;
                classSession.EndTime = classSession.EndTime;
                classSession.InstructorID = classSession.InstructorID;
                classSession.ClassRoomID = classSession.ClassRoomID;
                classSession.CourseID = classSession.CourseID;
            }
            else
            {
                classSession.WeekDay = "";
                classSession.StartTime = TimeOnly.MinValue;
                classSession.EndTime = TimeOnly.MinValue;
                classSession.InstructorID = 0;
                classSession.ClassRoomID = 0;
                classSession.CourseID= 0;
            }

            _context.ClassSessions.Add(classSession);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, classSession);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassSession(int id, [FromBody] ClassSession classSession)
        {
            if (id != classSession.ClassSessionID)
            {
                return BadRequest();
            }

            _context.Entry(classSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassSessionExists(id))
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
        public async Task<ActionResult> DeleteClassSession(int id)
        {
            var classSession = await _context.ClassSessions.FindAsync(id);

            if (classSession == null)
            {
                return NotFound();
            }

            _context.ClassSessions.Remove(classSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassSessionExists(int id)
        {
            return _context.ClassSessions.Any(e => e.ClassSessionID == id);
        }
    }
}
