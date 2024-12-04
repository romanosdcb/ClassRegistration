using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    public class ClassSessionMVMaint
    {
        public string? ErrorMessage { get; set; }
        public string? ScreenMessage { get; set; }
        public bool? ErrorCondition { get; set; }
        public bool? ShowScreenMessage { get; set; }

        public bool? ShowForInput { get; set; }
        public bool? ShowInstructorSchedule { get; set; }
        public bool? ShowRoomSchedule { get; set; }
        //public List<Instructor>? InstructorList { get; set; }
        //public List<ClassRoom>? ClassRoomList { get; set; }
        //public List<Course>? CourseList { get; set; }
        public string? PostInstruction { get; set; }

        public List<SelectListItem>? InstructorSelectList { get; set; }
        public List<SelectListItem>? ClassRoomSelectList { get; set; }
        public List<SelectListItem>? CourseSelectList { get; set; }
        public List<SelectListItem>? WeekDaySelectList { get; set; }
        //public List<ClassRoom>? BuildingList { get; set; }
        public List<SelectListItem>? BuildingSelectList { get; set; }
        public List<SelectListItem>? RoomSelectList { get; set; }
        public List<WorkingDailySchedule> InstructorSessions { get; set; }
        public List<WorkingDailySchedule> ClassRoomSessions { get; set; }

        public int? BuildingNumber { get; set; } = 0;
        public int? LastBuildingNumber { get; set; } = 0;

        public int ClassSessionID { get; set; }
        public string WeekDay { get; set; } = string.Empty;


        [BindProperty, DataType(DataType.Time)]
        public TimeOnly? StartTime { get; set; } = TimeOnly.MinValue;


        [BindProperty, DataType(DataType.Time)] 
        public TimeOnly? EndTime { get; set; } = TimeOnly.MaxValue;

        public int InstructorID { get; set; } = Int32.MinValue;
        public int ClassRoomID { get; set; } = Int32.MinValue;
        public int CourseID { get; set; } = Int32.MinValue;
    }
}
