using FirstMVCProject.DTOs;

namespace FirstMVCProject.Services.Interfaces
{
    public interface IBookApiClient
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto?> CreateBookAsync(CreateBookDto dto);
        Task<bool> UpdateBookAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteBookAsync(int id);
    }
}
