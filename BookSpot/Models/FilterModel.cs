using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSpot.Models
{
    public class FilterModel
    {
        [DisplayName("ISBN code")]
        public string? ISBNcode { get; set; }

        [DisplayName("Book title")]
        public string? BookTitle { get; set; }

        [DisplayName("Book author")]
        public string? Author { get; set; }

        [DisplayName("Book publisher")]
        public string? Publisher { get; set; }

        [DisplayName("Book publishing date")]
        public DateTime PublishingDate { get; set; }

        [DisplayName("Book genre")]
        public string? Genre { get; set; }

        [DisplayName("Book status")]
        public BookStatus Status { get; set; } = BookStatus.Available;
    }
}
