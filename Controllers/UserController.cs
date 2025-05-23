using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TravelReservationSystem.Data;
using TravelReservationSystem.Models;

namespace TravelReservationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // ✅ POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            ModelState.Remove("Reservations");

            if (!ModelState.IsValid)
                return View(user);

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Ky email është përdorur më parë.");
                return View(user);
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Role = user.Email.ToLower().Contains("admin") ? "Admin" : "User";
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // ✅ GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // ✅ POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                ViewBag.Error = "Email ose fjalëkalim i pasaktë.";
                return View();
            }

            // Ruaj në session për përdorim në të gjithë aplikacionin
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role);

            TempData["Message"] = "Mirsevjen, " + user.FullName;

            return RedirectToAction("Index", "Trip");
        }

        // ✅ Hashim fjalëkalimi
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // ✅ Dalje nga sistemi
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ✅ Paneli Admin
        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role.ToLower() != "admin")
                return RedirectToAction("Login");

            return View();
        }
        // GET: User/ManageReservations
        public async Task<IActionResult> ManageReservations()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");

            var pendingReservations = await _context.Reservations
                .Include(r => r.Trip)
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .ToListAsync();

            return View(pendingReservations);
        }

// POST: User/UpdateReservationStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReservationStatus(int id, string status)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            reservation.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageReservations");
        }
    }
}
