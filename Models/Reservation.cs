using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Flight_Reservation_System.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int SeatNo { get; set; }
        public DateTime DateReservation { get; set; }
        [Required]
        public int FlightId { get; set; }
        public Flights? Flights { get; set; } = null;
        [Required]

        public string? PassengerId { get; set; } = null;
        public Passenger? Passenger { get; set; }
    }
}
