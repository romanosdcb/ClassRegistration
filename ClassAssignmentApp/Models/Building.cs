using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    public class Building
    {
        [Key]
        public int BuildingID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        public string? Phone { get; set; }
    }
}
