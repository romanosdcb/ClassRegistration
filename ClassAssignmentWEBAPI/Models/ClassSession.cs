using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class ClassSession
    {
        [Key]
        public int ClassSessionID { get; set; }

        [Required]
        public string WeekDay { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public int InstructorID { get; set; }

        [Required]
        public int ClassRoomID { get; set; }

        [Required]
        public int CourseID { get; set; }
    }
}
