using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models.LmycBoatClub
{
    public class Boat
    {
        public int BoatId { get; set; }

        [Required]
        [Display(Name = "Boat Name")]
        public string BoatName { get; set; }
        public string Picture { get; set; }

        [Required]
        [Display(Name = "Length In Feet")]
        public double LengthInFeet { get; set; }
        public string Make { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        [ScaffoldColumn(false)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public ApplicationUser User { get; set; }
    }
}
