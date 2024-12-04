using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Models
{
    [Keyless]
    public class Schedule
    {
        public string RequestType { get; set; }
        public InstructorSchedule IREC { get; set; }
        public StudentSchedule SREC { get; set; }

        public List<InstructorSchedule> InstructorSessions { get; set; }
        public List<StudentSchedule> StudentSessions { get; set; }

    }

    [Keyless]
    public class InstructorSchedule
    {
        public int ClassSessionID { get; set; }
        public int InstructorID { get; set; }
        public int ClassRoomID { get; set; }
        public int CourseID { get; set; }
        public string InstructorName { get; set; }
        public string BuildingRoom { get; set; }
        public string CourseName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string WeekDay { get; set; }
        public int DayNumber { get; set; }
        public int StudentCount { get; set; }
    }

    [Keyless]
    public class StudentSchedule
    {
        public int ClassSessionID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public int InstructorID { get; set; }
        public int DayNumber { get; set; }
        public string WeekDay { get; set; }
        public string CourseName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string BuildingRoom { get; set; }
        public string InstructorName { get; set; }
    }
}
