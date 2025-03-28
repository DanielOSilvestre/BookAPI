using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using BookInformationAggregatorAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using BookInformationAggregatorAPI.Services;

namespace BookInformationAggregatorAPI.Tests
{
    public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly BookService _bookService;
        private readonly BooksController _controller;

        public BooksControllerTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();

            _bookService = new BookService();

            _controller = new BooksController(_httpClient, _bookService);
        }

        [Theory]
        [InlineData("/books", System.Net.HttpStatusCode.OK)]
        public async Task GetBooksTests(string endpoint, System.Net.HttpStatusCode expectedStatusCode)
        {
            var result = _controller.GetAllBooks();

            var okResult = Assert.IsType<OkResult>(result);

        }
    }
}