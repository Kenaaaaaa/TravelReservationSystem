using Microsoft.AspNetCore.Mvc;
using TravelReservationSystem.Data;
using TravelReservationSystem.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

namespace TravelReservationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: User/Register
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

            // Vendos rolin në varësi të përmbajtjes së emailit
            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Role = user.Email.ToLower().Contains("admin") ? "admin" : "user";
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // GET: User/Login
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

            // Ruaj të dhënat në session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role); // e merr nga databaza

            TempData["Message"] = "Mirsevjen, " + user.FullName;
            return RedirectToAction("Index", "Trip");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");

            return View();
        }
    }
}
