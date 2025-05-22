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

        // GET: /Reservation/MyReservations
        public IActionResult MyReservations()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var reservations = _context.Reservations
                .Include(r => r.Trip)
                .Where(r => r.UserId == userId)
                .ToList();

            return View(reservations);
        }

        // GET: /Reservation/Create?tripId=1
        public async Task<IActionResult> Create(int tripId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
            {
                return NotFound();
            }

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
    }
}