using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelReservationSystem.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Destinacioni është i detyrueshëm.")]
        [StringLength(100, ErrorMessage = "Destinacioni nuk mund të ketë më shumë se 100 karaktere.")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Përshkrimi është i detyrueshëm.")]
        [StringLength(1000, ErrorMessage = "Përshkrimi është shumë i gjatë.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Çmimi është i detyrueshëm.")]
        [Range(1, 10000, ErrorMessage = "Çmimi duhet të jetë mes 1 dhe 10000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Numri i ditëve është i detyrueshëm.")]
        [Range(1, 365, ErrorMessage = "Ditët duhet të jenë mes 1 dhe 365.")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Data e nisjes është e detyrueshme.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Linku i fotos është i detyrueshëm.")]
        [Url(ErrorMessage = "Ju lutem vendosni një URL të vlefshme.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Numri maksimal i personave është i detyrueshëm.")]
        [Range(1, 1000, ErrorMessage = "Numri maksimal duhet të jetë mes 1 dhe 1000.")]
        public int MaxPeople { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}