using FirstMVCProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FirstMVCProject.Controllers
{
    public class BooksApiController : Controller
    {
        private readonly IBookApiClient _bookApiClient;
        public BooksApiController(IBookApiClient bookApiClient)
        {
            _bookApiClient = bookApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _bookApiClient.GetAllBooksAsync();
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookApiClient.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
    }
}
