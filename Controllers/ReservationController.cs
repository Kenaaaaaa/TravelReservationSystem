using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelReservationSystem.Data;
using TravelReservationSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TravelReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /Reservation/MyReservations
        public IActionResult MyReservations()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var reservations = _context.Reservations
                .Include(r => r.Trip)
                .Where(r => r.UserId == userId)
                .ToList();

            return View(reservations);
        }

        // ✅ GET: /Reservation/Create?tripId=1
        public async Task<IActionResult> Create(int tripId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
                return NotFound();

            var reservation = new Reservation
            {
                TripId = tripId,
                UserId = userId.Value,
                ReservationDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rezervimi u krye me sukses!";
            return RedirectToAction("MyReservations");
        }

        // ✅ GET: /Reservation/Pending
        public async Task<IActionResult> Pending()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || !role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Login", "User");

            var pendingReservations = await _context.Reservations
                .Include(r => r.Trip)
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .ToListAsync();

            return View(pendingReservations);
        }

        // ✅ POST: /Reservation/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || !role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Login", "User");

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            if (status != "Accepted" && status != "Refused")
                return BadRequest("Status i pavlefshëm");

            reservation.Status = status;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Rezervimi u {status.ToLower()} me sukses.";
            return RedirectToAction("Pending");
        }
    }
}
