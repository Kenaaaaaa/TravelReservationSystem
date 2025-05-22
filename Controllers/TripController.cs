using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelReservationSystem.Data;
using TravelReservationSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace TravelReservationSystem.Controllers
{
    public class TripController : Controller
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Trip
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips.ToListAsync();
            return View(trips);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trip trip)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            if (ModelState.IsValid)
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Udhëtimi u ruajt me sukses!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Gabim në validim: " + error.ErrorMessage);
            }

            return View(trip);
        }

        // GET: Trip/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();
            return View(trip);
        }

        // POST: Trip/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trip trip)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            if (id != trip.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trip);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Udhëtimi u përditësua me sukses!";
                return RedirectToAction(nameof(Index));
            }

            return View(trip);
        }

        // GET: Trip/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();
            return View(trip);
        }

        // POST: Trip/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Udhëtimi u fshi me sukses!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null) return NotFound();
            return View(trip);
        }

        // Helper method
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "admin";
        }
    }
}
