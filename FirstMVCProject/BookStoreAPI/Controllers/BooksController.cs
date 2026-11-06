using BookStoreAPI.Data;
using BookStoreAPI.Dtos;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookStoreDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all books from the database with optional search and genre filtering.
        /// Route: GET /api/Books/GetAll
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] string? search, [FromQuery] Genres? genre)
        {
            _logger.LogInformation("Fetching books. Filter search='{Search}', genre='{Genre}'", search, genre);

            var query = _context.Books.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                query = query.Where(b => b.BookName.Contains(trimmed) || b.Author.Contains(trimmed));
            }

            if (genre.HasValue)
            {
                query = query.Where(b => b.Genre == genre.Value);
            }

            var books = await query
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    BookName = b.BookName,
                    Author = b.Author,
                    Genre = b.Genre,
                    ImageUrl = b.ImageUrl
                })
                .ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book by its ID.
        /// Route: GET /api/Books/GetById/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching book with ID {Id}", id);

            var book = await _context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    BookName = b.BookName,
                    Author = b.Author,
                    Genre = b.Genre,
                    ImageUrl = b.ImageUrl
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                _logger.LogWarning("Book with ID {Id} was not found.", id);
                return NotFound(new { message = $"Book with ID {id} was not found." });
            }

            return Ok(book);
        }

        /// <summary>
        /// Creates a new book (text-based data only, no image upload operations).
        /// Route: POST /api/Books/Create
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            _logger.LogInformation("Creating new book: {BookName}", dto.BookName);

            var book = new Book
            {
                BookName = dto.BookName.Trim(),
                Author = dto.Author.Trim(),
                Genre = dto.Genre,
                ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var resultDto = new BookDto
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Genre = book.Genre,
                ImageUrl = book.ImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, resultDto);
        }

        /// <summary>
        /// Updates an existing book by its ID.
        /// Route: PUT /api/Books/Update/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            _logger.LogInformation("Updating book with ID {Id}", id);

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Cannot update: Book with ID {Id} does not exist.", id);
                return NotFound(new { message = $"Book with ID {id} was not found." });
            }

            book.BookName = dto.BookName.Trim();
            book.Author = dto.Author.Trim();
            book.Genre = dto.Genre;
            book.ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// Route: DELETE /api/Books/Delete/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting book with ID {Id}", id);

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Cannot delete: Book with ID {Id} does not exist.", id);
                return NotFound(new { message = $"Book with ID {id} was not found." });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
