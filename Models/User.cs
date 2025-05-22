using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelReservationSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri i plotë është i detyrueshëm.")]
        [StringLength(100, ErrorMessage = "Emri i plotë nuk mund të ketë më shumë se 100 karaktere.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email-i është i detyrueshëm.")]
        [EmailAddress(ErrorMessage = "Email-i nuk është në formatin e duhur.")]
        [StringLength(100, ErrorMessage = "Email-i nuk mund të ketë më shumë se 100 karaktere.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fjalëkalimi është i detyrueshëm.")]
        [MinLength(6, ErrorMessage = "Fjalëkalimi duhet të ketë të paktën 6 karaktere.")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User"; // "User" ose "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public ICollection<Reservation> Reservations { get; set; }
    }
}