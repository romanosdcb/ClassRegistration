using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    public class StudentSessionMVMaint
    {
        public string? ErrorMessage { get; set; }
        public string? ScreenMessage { get; set; }
        public bool? ErrorCondition { get; set; }
        public bool? ShowScreenMessage { get; set; }

        public bool? ShowForInput { get; set; }
        public bool? ShowStudentSchedule { get; set; }
        public bool? ShowCourseSchedule { get; set; }

        public List<SelectListItem>? StudentSelectList { get; set; }
        public List<SelectListItem>? CourseSelectList { get; set; }

        public List<WorkingDailySchedule> StudentSessions { get; set; }
        public List<WorkingDailySchedule> CourseSessions { get; set; }
        public List<CourseOfferings> CourseOfferings { get; set; }


        public int StudentClassSessionID { get; set; }
        public int StudentID { get; set; } = Int32.MinValue;
        public int? CourseID { get; set; } = Int32.MinValue;
        public string StudentName { get; set; }
        public string CourseName { get; set; } = String.Empty;


        public int? TimesAbsent { get; set; } = Int32.MinValue;
        public int ClassRoomID { get; set; } = Int32.MinValue;
        public int BuildingNumber { get; set; } = Int32.MinValue;
        public int InstructorID { get; set; } = Int32.MinValue;
        public string WeekDay { get; set; } = String.Empty;
    }
}
