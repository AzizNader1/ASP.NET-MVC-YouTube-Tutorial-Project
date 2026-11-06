using System.ComponentModel.DataAnnotations;
using BookStoreAPI.Models;

namespace BookStoreAPI.Dtos
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Book name is required.")]
        [StringLength(100, ErrorMessage = "Book name cannot exceed 100 characters.")]
        public string BookName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required.")]
        public Genres Genre { get; set; }

        /// <summary>
        /// Text-based image URL or path (stores text directly, no file upload operations).
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }
}
