using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models.LmycBoatClub
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Start Date and Time is required.")]
        [DisplayName("From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End Date and Time is required.")]
        [DisplayName("To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [ForeignKey("User")]
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey("Boat")]
        public int BoatId { get; set; }

        [DisplayName("Boat Name")]
        public Boat Boat { get; set; }
    }
}
