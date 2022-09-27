using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSpot.Models
{
    public class BookView
    {
        public IEnumerable<Book>? Books { get; set; }
        public Book? Filter { get; set; }

        public SelectList? Genres { get; set; }
        //public string? MovieGenre { get; set; }
        //public string? SearchString { get; set; }
    }
}
