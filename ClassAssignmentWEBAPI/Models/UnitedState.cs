using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class UnitedState
    {
        [Key]
        public int StateID { get; set; }

        [Required]
        [Display(Name = "State Code")]
        [StringLength(2)]
        public string StateAbbreviation { get; set; }

        [Required]
        [Display(Name = "State Name")]
        [StringLength(50)]
        public string StateName { get; set; }
    }
}
