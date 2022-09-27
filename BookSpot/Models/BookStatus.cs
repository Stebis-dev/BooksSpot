namespace BookSpot.Models
{
    public class BookStatus
    {
        public BookStatus()
        {
            Index = 0;
        }
        public int Index { get; set; }
        public List<string> States = new List<string>{ "Available", "Reserved", "Borrowed"};
        
}
}
