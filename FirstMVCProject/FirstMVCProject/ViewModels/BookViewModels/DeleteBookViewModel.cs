using FirstMVCProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FirstMVCProject.ViewModels.BookViewModels
{
    public class DeleteBookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book name is required.")]
        [MinLength(5, ErrorMessage = "Book name must be at least 5 characters long.")]
        [MaxLength(100, ErrorMessage = "Book name cannot exceed 100 characters.")]
        public string BookName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Author is required.")]
        [MinLength(5, ErrorMessage = "Author name must be at least 5 characters long.")]
        [MaxLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string Author { get; set; } = string.Empty;
        [Required(ErrorMessage = "Genre is required.")]
        public Genres Genre { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
