using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentApp.Models
{
    public class ClassSession
    {
        [Key]
        public int ClassSessionID { get; set; }

        [Required]
        [Display(Name = "Day Of Week")]
        [StringLength(10)]
        public string WeekDay { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Start Time")]
        public TimeOnly StartTime { get; set; } = TimeOnly.MinValue;

        //[Required]
        [Display(Name = "End Time")]
        public TimeOnly EndTime { get; set; } = TimeOnly.MinValue;

        //[Required]
        [Display(Name = "Instructor ID")]
        public int InstructorID { get; set; } = Int32.MinValue;

        //[Required]
        [Display(Name = "Classroom ID")]
        public int ClassRoomID { get; set; } = Int32.MinValue;

        //[Required]
        [Display(Name = "Course ID")]
        public int CourseID { get; set; } = Int32.MinValue;
    }
}
