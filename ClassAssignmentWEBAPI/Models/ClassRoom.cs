using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class ClassRoom
    {
        [Key]
        public int ClassRoomID { get; set; }

        [Required]
        public int BuildingNumber { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        //public required string Content { get; set; }
        [Required]
        public int Capacity { get; set; }

        public string? Unavailable { get; set; }
    }
}
