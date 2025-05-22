using System;
using TravelReservationSystem.Models;

namespace TravelReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public int NumPeople { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Active"; // ose Canceled, Completed
    }
}