using FirstMVCProject.Data;
using FirstMVCProject.Models;
using FirstMVCProject.ViewModels.BookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstMVCProject.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly string[] PermittedExtensions = [".jpg", ".jpeg", ".png"];
        private const long FileSizeLimit = 5 * 1024 * 1024; // 5 MB

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewData["BooksCount"] = await _context.Books.CountAsync();
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateBookViewModel createBookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createBookViewModel);
            }

            if (createBookViewModel.ImageFile == null || createBookViewModel.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image file.");
                return View(createBookViewModel);
            }

            var result = await ValidateAndSaveImageAsync(createBookViewModel.ImageFile);
            var imageUrl = "";
            if (result == "File size exceeds the limit of 5 MB.")
            {
                ModelState.AddModelError("ImageFile", result);
                return View(createBookViewModel);
            }
            else if (result == "Invalid file type. Please upload a JPG, JPEG, or PNG file.")
            {
                ModelState.AddModelError("ImageFile", result);
                return View(createBookViewModel);
            }
            else if (result == "Please select an image file.")
            {
                ModelState.AddModelError("ImageFile", result);
                return View(createBookViewModel);
            }
            else
            {
                imageUrl = result;
            }

            var book = new Book
            {
                BookName = createBookViewModel.BookName,
                Author = createBookViewModel.Author,
                Genre = createBookViewModel.Genre,
                ImageUrl = imageUrl
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Book created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var editBookViewModel = new EditBookViewModel
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Genre = book.Genre,
                ImageUrl = book.ImageUrl
            };

            return View(editBookViewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id, EditBookViewModel editBookViewModel)
        {
            if (id == null || id != editBookViewModel.Id)
            {
                return NotFound();
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                editBookViewModel.ImageUrl = existingBook.ImageUrl;
                return View(editBookViewModel);
            }

            // Check if a new image was uploaded
            var imageFile = editBookViewModel.ImageFile ?? (Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null);
            if (imageFile != null && imageFile.Length > 0)
            {
                // Delete the old image file if one exists
                DeleteImageFile(existingBook.ImageUrl);

                // Save the newly uploaded image
                var imageUrl = await ValidateAndSaveImageAsync(imageFile);
                existingBook.ImageUrl = imageUrl;
            }

            try
            {
                existingBook.BookName = editBookViewModel.BookName;
                existingBook.Author = editBookViewModel.Author;
                existingBook.Genre = editBookViewModel.Genre;

                _context.Update(existingBook);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Book updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(existingBook.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var deleteBookViewModel = new DeleteBookViewModel
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Genre = book.Genre,
                ImageUrl = book.ImageUrl
            };

            return View(deleteBookViewModel);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // 1. Delete physical image file from wwwroot
            DeleteImageFile(book.ImageUrl);

            // 2. Remove book entity from database
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int? id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private async Task<string> ValidateAndSaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "Please select an image file.";
            }

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!PermittedExtensions.Contains(extension))
            {
                return "Invalid file type. Please upload a JPG, JPEG, or PNG file.";
            }

            if (imageFile.Length > FileSizeLimit)
            {
                return "File size exceeds the limit of 5 MB.";
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "books");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/books/{fileName}";
        }

        private void DeleteImageFile(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var relativePath = imageUrl.TrimStart('~', '/', '\\');
            var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }
    }
}
