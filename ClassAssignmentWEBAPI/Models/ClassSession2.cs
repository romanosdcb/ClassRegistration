using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentWEBAPI.Models
{
    public class ClassSession2
    {
        [Key]
        public int ClassSessionID { get; set; }

        [Required]
        public string WeekDay { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public int InstructorID { get; set; }

        [Required]
        public int ClassRoomID { get; set; }

        [Required]
        public int CourseID { get; set; }
    }
}
