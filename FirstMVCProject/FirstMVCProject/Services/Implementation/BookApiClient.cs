using FirstMVCProject.DTOs;
using FirstMVCProject.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FirstMVCProject.Services.Implementation
{
    public class BookApiClient : IBookApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookApiClient> _logger;

        // Custom JSON options to handle case-insensitivity and string enums
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private const string BaseUrl = "https://localhost:7048/api/";

        public BookApiClient(HttpClient httpClient, ILogger<BookApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(BaseUrl);
            }
            else if (!_httpClient.BaseAddress.ToString().EndsWith("/"))
            {
                _httpClient.BaseAddress = new Uri(_httpClient.BaseAddress.ToString() + "/");
            }
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Books/GetAll");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("External API call to GetAll failed with status code: {StatusCode}", response.StatusCode);
                    return Enumerable.Empty<BookDto>();
                }

                var books = await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>(_jsonOptions);
                return books ?? Enumerable.Empty<BookDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all books from API.");
                return Enumerable.Empty<BookDto>();
            }
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Books/GetById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("External API call to GetById for ID {Id} returned status code: {StatusCode}", id, response.StatusCode);
                    return null;
                }

                var book = await response.Content.ReadFromJsonAsync<BookDto>(_jsonOptions);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching book with ID {Id} from API.", id);
                return null;
            }
        }

        public async Task<BookDto?> CreateBookAsync(CreateBookDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Books/Create", dto, _jsonOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("External API call to Create failed with status code: {StatusCode}. Details: {Error}", response.StatusCode, error);
                    return null;
                }

                var createdBook = await response.Content.ReadFromJsonAsync<BookDto>(_jsonOptions);
                return createdBook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating book via API.");
                return null;
            }
        }

        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Books/Update/{id}", dto, _jsonOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("External API call to Update book {Id} failed with status code: {StatusCode}. Details: {Error}", id, response.StatusCode, error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating book with ID {Id} via API.", id);
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Books/Delete/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("External API call to Delete book {Id} failed with status code: {StatusCode}. Details: {Error}", id, response.StatusCode, error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting book with ID {Id} via API.", id);
                return false;
            }
        }
    }
}
