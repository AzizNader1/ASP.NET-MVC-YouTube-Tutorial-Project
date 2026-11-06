using BookStoreAPI.Models;

namespace BookStoreAPI.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Genres Genre { get; set; }
        public string GenreName => Genre.ToString();
        public string ImageUrl { get; set; } = string.Empty;
    }
}
