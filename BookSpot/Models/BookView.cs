using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSpot.Models
{
    public class BookView
    {
        public IEnumerable<Book>? Books { get; set; }

        public FilterModel? Filter { get; set; }
    }
}
