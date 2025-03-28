using BookInformationAggregatorAPI.DTOs;
using BookInformationAggregatorAPI.ExternalModels;
using BookInformationAggregatorAPI.Models;
using BookInformationAggregatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly HttpClient _httpClient;

        public BooksController(HttpClient httpClient, BookService bookService)
        {
            _httpClient = httpClient;
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            Book? result = _bookService.GetBookById(id);
            return result != null ? Ok(result) : NotFound(new { message = $"Book with id {id} was not found." });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBookByTitleOrAuthor(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest(new { message = "No query parameter provided." });
            }

            var openLibraryTitleResponse = await _httpClient.GetStringAsync($"https://openlibrary.org/search.json?title={query}");
            var openLibraryAuthorResponse = await _httpClient.GetStringAsync($"https://openlibrary.org/search.json?author={query}");

            if (string.IsNullOrEmpty(openLibraryTitleResponse) && string.IsNullOrEmpty(openLibraryAuthorResponse))
            {
                return NotFound(new { message = "Empty response from Open Library."});
            }

            var titlesFound = JsonSerializer.Deserialize<OpenLibraryResponse>(openLibraryTitleResponse);
            var authorsFound = JsonSerializer.Deserialize<OpenLibraryResponse>(openLibraryAuthorResponse);

            List<Book> resultBooks = [];
            bool noTitlesFound = false;

            if ((titlesFound?.Docs != null && titlesFound.NumFound != 0))
            {
                resultBooks.AddRange(titlesFound.Docs.Select(b => new Book
                {
                    Id = b.Key ?? "Key not given",
                    Title = b.Title,
                    Author = b.AuthorName?.FirstOrDefault() ?? "Unknown Author",
                    PublishedYear = b.FirstPublicYear
                }).ToList());
            }
            else
            {
                noTitlesFound = true;
            }

            if ((authorsFound?.Docs == null || authorsFound.NumFound == 0) && noTitlesFound)
            {
                return NotFound(new { message = $"No books found with the title or author: {query}" });
            }
            else
            {
                resultBooks.AddRange(authorsFound.Docs.Select(b => new Book
                {
                    Id = b.Key ?? "Key not given",
                    Title = b.Title,
                    Author = b.AuthorName?.FirstOrDefault() ?? "Unknown Author",
                    PublishedYear = b.FirstPublicYear
                }).ToList());
            }

            return Ok(resultBooks);


        }

        [HttpPost]
        public ActionResult<Book> AddBook([FromBody] CreateBookDTO bookDTO)
        {
            bool success = _bookService.AddBook(bookDTO, out Book? createdBook);

            if (!success)
            {
                return BadRequest(new { message = "Invalid data. Check title and author as both are required." });
            }

            return CreatedAtAction(nameof(GetBook), new { id = createdBook?.Id }, createdBook);
        }
    }
 
}
