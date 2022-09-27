using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookSpot.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("ISBN code")]
        public string? ISBNcode { get; set; }

        [Required]
        [DisplayName("Book title")]
        public string? BookTitle { get; set; }

        [Required]
        [DisplayName("Book author")]
        public string? Author { get; set; }

        [Required]
        [DisplayName("Book publisher")]
        public string? Publisher { get; set; }

        [DisplayName("Book publishing date")]
        public DateTime PublishingDate { get; set; }

        [Required]
        [DisplayName("Book genre")]
        public string? Genre { get; set; }
        // can be a lot of diffrent genre books . . .

        // book status : rezerve, taken, is in shop . . .

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    }
}
