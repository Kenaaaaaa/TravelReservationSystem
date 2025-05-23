using Microsoft.EntityFrameworkCore;
using TravelReservationSystem.Models;
using TravelReservationSystem.Helpers;namespace TravelReservationSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}