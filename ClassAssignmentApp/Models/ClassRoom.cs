using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentApp.Models
{
    public class ClassRoom
    {
        [Key]
        public int ClassRoomID { get; set; }

        //[Required]
        [Display(Name ="Building Number")]
        public int? BuildingNumber { get; set; } = Int32.MinValue;

        //[Required]
        [Display(Name = "Room Number")]
        public int? RoomNumber { get; set; } = Int32.MinValue;

        //public required string Content { get; set; }
        //[Required]
        [Display(Name = "Max Students")]
        public int? Capacity { get; set; } = Int32.MinValue;

        [Display(Name = "Unavailable (Y/N)")]
        [StringLength(1)]
        public string? Unavailable { get; set; } = string.Empty;
    }
}
