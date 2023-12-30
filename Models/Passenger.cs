namespace Flight_Reservation_System.Models
{
    public class Passenger
    {
        public string PassengerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }//Navigation her Reservation birden fazla Passengeri olabilir

    }
}
