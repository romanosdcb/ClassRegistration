using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentApp.Models
{
    public class ScheduleMVMaint
    {
        public string RequestType { get; set; }
        public bool ShowInstructor { get; set; }
        public bool ShowInstructorData { get; set; }
        public bool ShowStudent { get; set; }
        public bool ShowStudentData { get; set; }

        public int InstructorID { get; set; }
        public int StudentID { get; set; }

        public List<SelectListItem>? InstructorSelectList { get; set; }
        public List<SelectListItem>? StudentSelectList { get; set; }


        public InstructorSchedule IREC { get; set; }
        public StudentSchedule SREC { get; set; }

        public List<InstructorSchedule> InstructorSessions { get; set; }
        public List<StudentSchedule> StudentSessions { get; set; }


        //[Keyless]
        //public class Schedule
        //{
 

        //    public List<InstructorSchedule> InstructorSessions { get; set; }
        //    public List<StudentSchedule> StudentSessions { get; set; }

        //}

    
    }

    //[Keyless]
    //public class InstructorSchedule
    //{
    //    public int ClassSessionID { get; set; }
    //    public int InstructorID { get; set; }
    //    public int ClassRoomID { get; set; }
    //    public int CourseID { get; set; }
    //    public string InstructorName { get; set; }
    //    public string BuildingRoom { get; set; }
    //    public string CourseName { get; set; }
    //    public TimeOnly StartTime { get; set; }
    //    public TimeOnly EndTime { get; set; }
    //    public string WeekDay { get; set; }
    //    public int DayNumber { get; set; }
    //    public int StudentCount { get; set; }
    //}

    //[Keyless]
    //public class StudentSchedule
    //{
    //    public int ClassSessionID { get; set; }
    //    public int StudentID { get; set; }
    //    public int CourseID { get; set; }
    //    public int InstructorID { get; set; }
    //    public int DayNumber { get; set; }
    //    public string WeekDay { get; set; }
    //    public string CourseName { get; set; }
    //    public TimeOnly StartTime { get; set; }
    //    public TimeOnly EndTime { get; set; }
    //    public string BuildingRoom { get; set; }
    //    public string InstructorName { get; set; }
    //}
}
