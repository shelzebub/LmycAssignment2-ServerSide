using AspNet.Security.OAuth.Validation;
using LmycWeb.Data;
using LmycWeb.Models;
using LmycWeb.Models.LmycBoatClub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Controllers.APIControllers
{
    [Produces("application/json")]
    [Route("api/BoatsAPI")]
    [Authorize(Policy = "LoginRequired", AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class BoatsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BoatsAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/BoatsAPI
        [HttpGet]
        public IEnumerable<Boat> GetBoats()
        {
            return _context.Boats;
        }

        // GET: api/BoatsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoat([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);

            if (boat == null)
            {
                return NotFound();
            }

            return Ok(boat);
        }

        // PUT: api/BoatsAPI/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> PutBoat([FromRoute] int id, [FromBody] Boat boat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != boat.BoatId)
            {
                return BadRequest();
            }

            _context.Entry(boat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BoatsAPI
        [HttpPost]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> PostBoat([FromBody] Boat boat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            boat.User = user ?? throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            _context.Boats.Add(boat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoat", new { id = boat.BoatId }, boat);
        }

        // DELETE: api/BoatsAPI/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> DeleteBoat([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);
            if (boat == null)
            {
                return NotFound();
            }

            _context.Boats.Remove(boat);
            await _context.SaveChangesAsync();

            return Ok(boat);
        }

        private bool BoatExists(int id)
        {
            return _context.Boats.Any(e => e.BoatId == id);
        }
    }
}
