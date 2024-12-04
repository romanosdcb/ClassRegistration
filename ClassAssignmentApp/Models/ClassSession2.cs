using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    public class ClassSession2
    {
        [Key]
        public int ClassSessionID { get; set; }

        [Required]
        [Display(Name = "Day Of Week")]
        [StringLength(10)]
        public string WeekDay { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; } = "";

        //[Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; } = "";

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
