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

        [DisplayName("Book status")]
        public BookStatus Status { get; set; } = BookStatus.Available;

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
