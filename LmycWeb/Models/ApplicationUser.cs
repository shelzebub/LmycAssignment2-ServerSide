using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Models.LmycBoatClub;
using Microsoft.AspNetCore.Identity;

namespace LmycWeb.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string MobileNumber { get; set; }
        public double SailingExperience { get; set; }

        public List<Boat> Boats { get; set; }
    }
}
