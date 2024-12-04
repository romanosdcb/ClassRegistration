using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/{ScheduleType}")]
        public async Task<ActionResult<Schedule>> GetSchedules(int id, string ScheduleType)
        {
            Schedule ScheduleMaint = new Schedule();
            ScheduleMaint.RequestType = ScheduleType;

            List<InstructorSchedule> InstructorSessions = new List<InstructorSchedule>();
            List<StudentSchedule> StudentSessions = new List<StudentSchedule>();
            InstructorSessions.Clear();
            StudentSessions.Clear();

            switch (ScheduleType)
            {
                case "Instructor":
                    var SQL1 = "EXEC InstructorWeeklySchedule " + id.ToString();
                    var InstructorDBSet = await _context.InstructorSchedules.FromSqlRaw(SQL1).ToListAsync();

                    foreach (var item in InstructorDBSet)
                    {
                        InstructorSchedule IREC = new InstructorSchedule();
                        IREC.ClassSessionID = item.ClassSessionID;
                        IREC.InstructorID = item.InstructorID;
                        IREC.ClassRoomID = item.ClassRoomID;
                        IREC.CourseID = item.CourseID;
                        IREC.InstructorName = item.InstructorName;
                        IREC.BuildingRoom = item.BuildingRoom;
                        IREC.CourseName = item.CourseName;
                        IREC.StartTime = item.StartTime;
                        IREC.EndTime = item.EndTime;
                        IREC.WeekDay = item.WeekDay;
                        IREC.DayNumber = item.DayNumber;
                        IREC.StudentCount = item.StudentCount;
                        InstructorSessions.Add(IREC);
                    }
                    break;

                case "Student":
                    var SQL2 = "EXEC StudentWeeklySchedule " + id.ToString();
                    var StudentDBSet = await _context.StudentSchedules.FromSqlRaw(SQL2).ToListAsync();

                    foreach (var item in StudentDBSet)
                    {
                        StudentSchedule SREC = new StudentSchedule();
                        SREC.ClassSessionID = item.ClassSessionID;
                        SREC.StudentID = item.StudentID;
                        SREC.CourseID = item.CourseID;
                        SREC.InstructorID = item.InstructorID;
                        SREC.DayNumber = (int)item.DayNumber;
                        SREC.WeekDay = item.WeekDay;
                        SREC.CourseName = item.CourseName;
                        SREC.StartTime = item.StartTime;
                        SREC.EndTime = item.EndTime;
                        SREC.BuildingRoom = item.BuildingRoom;
                        SREC.InstructorName = item.InstructorName;
                        StudentSessions.Add(SREC);
                    }
                    break;

                default:

                    return ScheduleMaint;
            }

            ScheduleMaint.InstructorSessions = InstructorSessions;
            ScheduleMaint.StudentSessions = StudentSessions;

            return ScheduleMaint;
        }










    }
}
