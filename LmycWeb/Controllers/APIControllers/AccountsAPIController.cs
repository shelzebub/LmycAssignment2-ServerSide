using LmycWeb.Models;
using LmycWeb.Models.AccountViewModels;
using LmycWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Controllers.APIControllers
{
    [Produces("application/json")]
    [Route("api/AccountsAPI")]
    public class AccountsAPIController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountsAPIController(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<AccountsAPIController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        // POST api/AccountAPI/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                StreetAddress = model.StreetAddress,
                City = model.City,
                Province = model.Province,
                PostalCode = model.PostalCode,
                Country = model.Country,
                MobileNumber = model.MobileNumber,
                SailingExperience = model.SailingExperience
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            _logger.LogInformation("User created a new account with password.");
            await _userManager.AddToRoleAsync(user, "Member");

            return Ok();
        }
    }
}
