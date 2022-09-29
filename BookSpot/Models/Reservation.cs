namespace BookSpot.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int BookId { get; set; }
        public string AppUserId { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
