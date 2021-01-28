using BooksWebApp.Controllers;
using BooksWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace BooksWebAppTests
{
  public class BooksControllerTest
  {
    public BooksControllerTest()
    {
      InitContext();
    }

    private BooksContext _booksContext;

    private void InitContext()
    {
      var builder = new DbContextOptionsBuilder<BooksContext>()
          .UseInMemoryDatabase(databaseName: "database_name");

      var context = new BooksContext(builder.Options);
    
      _booksContext = context;
    }

    [Fact]
    public void TestGetBookById()
    {
      var books = Enumerable.Range(1, 10)
          .Select(i => new Book { BookId = i, Title = $"Sample{i}", Publisher = "Wrox Press" });
      _booksContext.Books.AddRange(books);
      int changed = _booksContext.SaveChanges();

      string expectedTitle = "Sample2";
      var controller = new BooksController(_booksContext);
      Book result = controller.Get(2);
      Assert.Equal(expectedTitle, result.Title);
    }

    [Fact]
    public void TestCreateBook()
    {
      var controller = new BooksController(_booksContext);
      var book = new Book { BookId = 1, Title = "Test", Publisher = "Ash" };
      controller.Post(book);
      Assert.Equal(1, _booksContext.Books.Count());
    }

    [Fact]
    public void TestDeleteBook()
    {
      var books = Enumerable.Range(1, 10)
           .Select(i => new Book { BookId = i, Title = $"Sample{i}", Publisher = "Wrox Press" });
      _booksContext.Books.AddRange(books);
      int changed = _booksContext.SaveChanges();

            var controller = new BooksController(_booksContext);
      var book = new Book { BookId = 1, Title = "Sample1", Publisher = "Wrox Press" };
      controller.Delete(1);
      Assert.Equal(9, _booksContext.Books.Count());
    }
  }
}
