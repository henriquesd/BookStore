using System;
using System.Collections.Generic;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Moq;
using Xunit;

namespace BookStore.Domain.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepositoryMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfBook_WhenBooksExist()
        {
            var books = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetAll()).ReturnsAsync(books);

            var result = await _bookService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenBooksDoNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetAll())
                .ReturnsAsync((List<Book>)null);

            var result = await _bookService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _bookRepositoryMock.Setup(c => c.GetAll())
                .ReturnsAsync(new List<Book>());

            await _bookService.GetAll();

            _bookRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnBook_WhenBookExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c => c.GetById(book.Id))
                .ReturnsAsync(book);

            var result = await _bookService.GetById(book.Id);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetById(1))
                .ReturnsAsync((Book)null);

            var result = await _bookService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _bookRepositoryMock.Setup(c => c.GetById(1))
                .ReturnsAsync(new Book());

            await _bookService.GetById(1);

            _bookRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnAListOfBook_WhenBooksWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetBooksByCategory(2))
                .ReturnsAsync(bookList);

            var result = await _bookService.GetBooksByCategory(2);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnNull_WhenBooksWithSearchedCategoryDoNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetBooksByCategory(2))
                .ReturnsAsync((IEnumerable<Book>)null);

            var result = await _bookService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldCallGetBooksByCategoryFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetBooksByCategory(2))
                .ReturnsAsync(bookList);

            await _bookService.GetBooksByCategory(2);

            _bookRepositoryMock.Verify(mock => mock.GetBooksByCategory(2), Times.Once);
        }

        [Fact]
        public async void Search_ShouldReturnAListOfBook_WhenBooksWithSearchedNameExist()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                c.Search(c => c.Name.Contains(bookName))).ReturnsAsync(bookList);

            var result = await _bookService.Search(searchedBook.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenBooksWithSearchedNameDoNotExist()
        {
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                    c.Search(c => c.Name.Contains(bookName)))
                .ReturnsAsync((IEnumerable<Book>)(null));

            var result = await _bookService.Search(searchedBook.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                    c.Search(c => c.Name.Contains(bookName)))
                .ReturnsAsync(bookList);

            await _bookService.Search(searchedBook.Name);

            _bookRepositoryMock.Verify(mock => mock.Search(c => c.Name.Contains(bookName)), Times.Once);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnAListOfBook_WhenBooksWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchBookWithCategory(searchedBook.Name))
                .ReturnsAsync(bookList);

            var result = await _bookService.SearchBookWithCategory(searchedBook.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnNull_WhenBooksWithSearchedCategoryDoNotExist()
        {
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchBookWithCategory(searchedBook.Name))
                .ReturnsAsync((IEnumerable<Book>)null);

            var result = await _bookService.SearchBookWithCategory(searchedBook.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldCallSearchBookWithCategoryFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.SearchBookWithCategory(searchedBook.Name))
                .ReturnsAsync(bookList);

            await _bookService.SearchBookWithCategory(searchedBook.Name);

            _bookRepositoryMock.Verify(mock => mock.SearchBookWithCategory(searchedBook.Name), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddBook_WhenBookNameDoesNotExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.Search(c => c.Name == book.Name))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.Add(book));

            var result = await _bookService.Add(book);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddBook_WhenBookNameAlreadyExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>() { book };

            _bookRepositoryMock.Setup(c =>
                c.Search(c => c.Name == book.Name))
                .ReturnsAsync(bookList);

            var result = await _bookService.Add(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.Search(c => c.Name == book.Name))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.Add(book));

            await _bookService.Add(book);

            _bookRepositoryMock.Verify(mock => mock.Add(book), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateBook_WhenBookNameDoesNotExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.Search(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.Update(book));

            var result = await _bookService.Update(book);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateBook_WhenBookDoesNotExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>()
            {
                new Book()
                {
                    Id = 2,
                    Name = "Book Test 2",
                    Author = "Author Test 2"
                }
            };

            _bookRepositoryMock.Setup(c =>
                c.Search(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(bookList);

            var result = await _bookService.Update(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.Search(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(new List<Book>());

            await _bookService.Update(book);

            _bookRepositoryMock.Verify(mock => mock.Update(book), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnTrue_WhenBookCanBeRemoved()
        {
            var book = CreateBook();

            var result = await _bookService.Remove(book);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            await _bookService.Remove(book);

            _bookRepositoryMock.Verify(mock => mock.Remove(book), Times.Once);
        }

        private Book CreateBook()
        {
            return new Book()
            {
                Id = 1,
                Name = "Book Test",
                Author = "Author Test",
                Description = "Description Test",
                Value = 10,
                CategoryId = 1,
                PublishDate = DateTime.MinValue.AddYears(40)
            };
        }

        private List<Book> CreateBookList()
        {
            return new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 1",
                    Author = "Author Test 1",
                    Description = "Description Test 1",
                    Value = 10,
                    CategoryId = 1
                },
                new Book()
                {
                    Id = 2,
                    Name = "Book Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    Value = 20,
                    CategoryId = 1
                },
                new Book()
                {
                    Id = 3,
                    Name = "Book Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    Value = 30,
                    CategoryId = 2
                }
            };
        }
    }
}