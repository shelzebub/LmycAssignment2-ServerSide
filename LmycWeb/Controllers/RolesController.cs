using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LmycWeb.Controllers
{
    [Authorize(Policy = "AdminRequired")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public RolesController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: Roles/Index
        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles;
            return View(await roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return BadRequest();
            }

            var users = _userManager.GetUsersInRoleAsync(role.Name).Result;

            var model = new RoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Users = users
            };

            return View(model);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(role.Name))
                {
                    return View();
                }
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result;

            var users = await _userManager.Users.ToListAsync();

            foreach (var u in usersInRole)
            {
                if (users.Contains(u))
                {
                    users.Remove(u);
                }
            }

            var model = new RoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Users = users
            };

            return View(model);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role.Name.Equals("Admin") || role == null)
            {
                return NotFound();
            }

            var users = _userManager.GetUsersInRoleAsync(role.Name).Result;
            var model = new RoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Users = users
            };

            return View(model);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role.Name.Equals("Admin") || role == null)
            {
                return BadRequest();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Roles/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (!result.Succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(Edit), new { id = model.RoleId });
        }

        // POST: Roles/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(RoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user.UserName.Equals("a") || user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (!result.Succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(Details), new { id = model.RoleId });
        }
    }
}