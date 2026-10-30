namespace FirstMVCProject.Models
{
    public enum Genres
    {
        Fiction,
        NonFiction,
        Mystery,
        Romance,
        ScienceFiction,
        Fantasy,
        Biography,
        History,
        SelfHelp,
        Poetry
    }
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Genres Genre { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
