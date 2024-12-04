using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class StudentClassSession
    {
        [Key]
        public int StudentClassSessionID { get; set; }

        [Required]
        [Display(Name = "Class Session ID")]
        public int ClassSessionID { get; set; } = Int32.MinValue;

        [Required]
        [Display(Name = "Student ID")]
        public int StudentID { get; set; } = Int32.MinValue;

        [Display(Name = "Times Absent")]
        public int? TimesAbsent { get; set; } = Int32.MinValue;
    }
}
