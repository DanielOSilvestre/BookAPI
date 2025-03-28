using BookInformationAggregatorAPI.DTOs;
using BookInformationAggregatorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookInformationAggregatorAPI.Services
{
    public class BookService
    {
        private List<Book> books = [];

        public BookService()
        {
            var booksJsonFilePath = Path.Combine(AppContext.BaseDirectory, "books.json");

            if (File.Exists(booksJsonFilePath))
            {
                var json = File.ReadAllText(booksJsonFilePath);
                books = JsonSerializer.Deserialize<List<Book>>(json) ?? [];
            }
        }

        public List<Book> GetAllBooks(){
            return books;
        }

        public Book? GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id.ToString());
        }

        public bool AddBook(CreateBookDTO newBook, out Book? createdBook)
        {
            createdBook = null;

            if(string.IsNullOrEmpty(newBook.Title) || string.IsNullOrEmpty(newBook.Author))
            {
                return false;
            }

            if(newBook.PublishedYear > DateTime.Now.Year)
            {
                return false;
            }

            int newId = books.Count != 0 ? books.Max(b => int.Parse(b.Id)) + 1 : 1;

            createdBook = new()
            { 
                Id = newId.ToString(),
                Title = newBook.Title,
                Author = newBook.Author,
                Description = newBook.Description,
                PublishedYear = newBook.PublishedYear
            };

            books.Add(createdBook);
            return true;
        }
    }
}
