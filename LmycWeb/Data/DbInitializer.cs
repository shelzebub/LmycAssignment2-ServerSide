using LmycWeb.Models;
using LmycWeb.Models.LmycBoatClub;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var admin = new ApplicationUser
                {
                    UserName = "a",
                    Email = "a@a.a",
                    FirstName = "Jason",
                    LastName = "Chen",
                    StreetAddress = "3700 Willingdon Ave",
                    City = "Burnaby",
                    Province = "BC",
                    PostalCode = "V5G 3H2",
                    Country = "Canada",
                    MobileNumber = "(604) 434-5734",
                    SailingExperience = 50
                };

                var adminId = await SeedingUser(serviceProvider, admin, "P@$$w0rd");

                await SeedingRole(serviceProvider, adminId, "Admin");

                var member = new ApplicationUser
                {
                    UserName = "m",
                    Email = "m@m.m",
                    FirstName = "Siri",
                    LastName = "Apple",
                    StreetAddress = "3700 Willingdon Ave",
                    City = "Burnaby",
                    Province = "BC",
                    PostalCode = "V5G 3H2",
                    Country = "Canada",
                    MobileNumber = "(604) 434-5734",
                    SailingExperience = 2
                };

                var memberId = await SeedingUser(serviceProvider, member, "P@$$w0rd");

                await SeedingRole(serviceProvider, memberId, "Member");

                if (context.Boats.Any())
                {
                    return;
                }

                foreach (var boat in GetBoats(adminId))
                {
                    context.Boats.Add(boat);
                }

                context.SaveChanges();
            }
        }

        private static async Task<string> SeedingUser(IServiceProvider serviceProvider, ApplicationUser newUser, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(newUser.UserName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    StreetAddress = newUser.StreetAddress,
                    City = newUser.City,
                    Province = newUser.Province,
                    PostalCode = newUser.PostalCode,
                    Country = newUser.Country,
                    MobileNumber = newUser.MobileNumber,
                    SailingExperience = newUser.SailingExperience
                };

                await userManager.CreateAsync(user, password);
            }
            return user.Id;
        }


        public static async Task<IdentityResult> SeedingRole(IServiceProvider serviceProvider, string uid, string role)
        {
            IdentityResult result = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                result = await roleManager.CreateAsync(new IdentityRole(role));
            }
        
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(uid);
            
            result = await userManager.AddToRoleAsync(user, role);

            return result;
        }


        private static List<Boat> GetBoats(string adminId)
        {
            List<Boat> boats = new List<Boat>()
            {
                new Boat()
                {
                    BoatName = "King George",
                    Picture = "http://www.lmyc.ca/sites/default/files/content/pages/sharqui%20photo.jpg",
                    LengthInFeet = 33,
                    Make = "LMYC",
                    Year = 2016,
                    CreationDate = DateTime.Today,
                    CreatedBy = adminId
                },

                new Boat()
                {
                    BoatName = "QueensBorough",
                    Picture = "http://www.lmyc.ca/sites/default/files/content/pages/white%20swan.jpg",
                    LengthInFeet = 50,
                    Make = "Vancouvoat",
                    Year = 2018,
                    CreationDate = DateTime.Today,
                    CreatedBy = adminId
                },

                new Boat()
                {
                    BoatName = "Otter",
                    Picture = "http://www.lmyc.ca/sites/default/files/Lightcurefleetpic.jpg",
                    LengthInFeet = 33,
                    Make = "The Otter Company",
                    Year = 2017,
                    CreationDate = DateTime.Today,
                    CreatedBy = adminId
                },

                new Boat()
                {
                    BoatName = "WaffleBoat",
                    Picture = "http://www.lmyc.ca/sites/default/files/content/pages/frankie.jpg",
                    LengthInFeet = 28,
                    Make = "Waffle Incorporations",
                    Year = 2018,
                    CreationDate = DateTime.Today,
                    CreatedBy = adminId
                },

                new Boat()
                {
                    BoatName = "KoiSailer",
                    Picture = "http://www.lmyc.ca/sites/default/files/content/pages/Small%20pegasus.jpg",
                    LengthInFeet = 22,
                    Make = "Koi & Co.",
                    Year = 2018,
                    CreationDate = DateTime.Today,
                    CreatedBy = adminId
                },

            };
            return boats;
        }
    }
}
