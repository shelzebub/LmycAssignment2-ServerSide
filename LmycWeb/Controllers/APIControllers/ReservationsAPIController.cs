using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models.LmycBoatClub;
using LmycWeb.Models;

namespace LmycWeb.Controllers.APIControllers
{
    [Produces("application/json")]
    [Route("api/ReservationsAPI")]
    public class ReservationsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ReservationsAPI
        [HttpGet]
        public IEnumerable<Reservation> GetReservation()
        {
            return _context.Reservation;
        }

        // GET: api/ReservationsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/ReservationsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation([FromRoute] int id, [FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ReservationId)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/ReservationsAPI
        [HttpPost]
        public async Task<IActionResult> PostReservation([FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.ReservationId }, reservation);
        }

        // DELETE: api/ReservationsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id, [FromRoute] ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null || !reservation.User.UserName.Equals(user.UserName))
            {
                return NotFound();
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationId == id);
        }
    }
}