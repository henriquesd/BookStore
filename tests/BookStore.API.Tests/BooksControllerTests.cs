using System;
using System.Collections.Generic;
using AutoMapper;
using BookStore.API.Controllers;
using BookStore.API.Dtos.Book;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.API.Tests
{
    public class BooksControllerTests
    {
        private readonly BooksController _booksController;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public BooksControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _mapperMock = new Mock<IMapper>();
            _booksController = new BooksController(_mapperMock.Object, _bookServiceMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistBooks()
        {
            var books = CreateBookList();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }
     
        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyBook()
        {
            var books = new List<Book>();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_JustOnce()
        {
            var books = CreateBookList();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            await _booksController.GetAll();

            _bookServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenBookExist()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            var result = await _booksController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync((Book)null);

            var result = await _booksController.GetById(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_JustOnce()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            await _booksController.GetById(2);

            _bookServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnOk_WhenBookWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<IEnumerable<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetBooksByCategory(book.CategoryId);
            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnNotFound_WhenBookWithSearchedCategoryDoesNotExist()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(new List<Book>());
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            var result = await _booksController.GetBooksByCategory(book.CategoryId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldCallGetBooksByCategoryFromService_JustOnce()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<IEnumerable<Book>>())).Returns(dtoExpected);

            await _booksController.GetBooksByCategory(book.CategoryId);

            _bookServiceMock.Verify(mock => mock.GetBooksByCategory(book.CategoryId), Times.Once);
        }

        [Fact]
        public async void Search_ShouldReturnOk_WhenBookWithSearchedNameExist()
        {
            var bookList = CreateBookList();
            var book = CreateBook();

            _bookServiceMock.Setup(c => c.Search(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<List<Book>>(It.IsAny<IEnumerable<Book>>())).Returns(bookList);

            var result = await _booksController.Search(book.Name);
            var actual = (OkObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldReturnNotFound_WhenBookWithSearchedNameDoesNotExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>();

            var dtoExpected = MapModelToBookResultDto(book);
            book.Name = dtoExpected.Name;

            _bookServiceMock.Setup(c => c.Search(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<Book>>(It.IsAny<Book>())).Returns(bookList);

            var result = await _booksController.Search(book.Name);
            var actual = (NotFoundObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromService_JustOnce()
        {
            var bookList = CreateBookList();
            var book = CreateBook();

            _bookServiceMock.Setup(c => c.Search(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<List<Book>>(It.IsAny<IEnumerable<Book>>())).Returns(bookList);

            await _booksController.Search(book.Name);

            _bookServiceMock.Verify(mock => mock.Search(book.Name), Times.Once);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnOk_WhenBookWithSearchedValueExist()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var bookResultList = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.SearchBookWithCategory(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<Book>>(It.IsAny<List<Book>>())).Returns(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(bookResultList);

            var result = await _booksController.SearchBookWithCategory(book.Name);
            var actual = (OkObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldCallSearchBookWithCategoryFromService_JustOnce()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var bookResultList = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.SearchBookWithCategory(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<Book>>(It.IsAny<List<Book>>())).Returns(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(bookResultList);

            await _booksController.SearchBookWithCategory(book.Name);

            _bookServiceMock.Verify(mock => mock.SearchBookWithCategory(book.Name), Times.Once);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnNotFound_WhenBookWithSearchedValueDoesNotExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>();

            _bookServiceMock.Setup(c => c.SearchBookWithCategory(book.Name)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<Book>>(It.IsAny<List<Book>>())).Returns(bookList);

            var result = await _booksController.SearchBookWithCategory(book.Name);
            var actual = (NotFoundObjectResult)result.Result;

            Assert.Equal("None book was founded", actual.Value);
            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async void Add_ShouldReturnOk_WhenBookIsAdded()
        {
            var book = CreateBook();
            var bookAddDto = new BookAddDto() { Name = book.Name };
            var bookResultDto = new BookResultDto()
            {
                Id = book.Id,
                Name = book.Name,
                Value = book.Value,
                Author = book.Author,
                Description = book.Description,
                PublishDate =  book.PublishDate,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name
            };

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookAddDto>())).Returns(book);
            _bookServiceMock.Setup(c => c.Add(book)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(bookResultDto);

            var result = await _booksController.Add(bookAddDto);
            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var bookAddDto = new BookAddDto();
            _booksController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _booksController.Add(bookAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenBookResultIsNull()
        {
            var book = CreateBook();
            var bookAddDto = new BookAddDto() { Name = book.Name };

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookAddDto>())).Returns(book);
            _bookServiceMock.Setup(c => c.Add(book)).ReturnsAsync((Book)null);

            var result = await _booksController.Add(bookAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromService_JustOnce()
        {
            var book = CreateBook();
            var bookAddDto = new BookAddDto() { Name = book.Name };

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookAddDto>())).Returns(book);
            _bookServiceMock.Setup(c => c.Add(book)).ReturnsAsync(book);

            await _booksController.Add(bookAddDto);

            _bookServiceMock.Verify(mock => mock.Add(book), Times.Once);
        }

        [Fact]
        public async void Update_ShouldReturnOk_WhenBookIsUpdatedCorrectly()
        {
            var book = CreateBook();
            var bookEditDto = new BookEditDto() { Id = book.Id,  Name = "Test" };

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookEditDto>())).Returns(book);
            _bookServiceMock.Setup(c => c.GetById(book.Id)).ReturnsAsync(book);
            _bookServiceMock.Setup(c => c.Update(book)).ReturnsAsync(book);

            var result = await _booksController.Update(bookEditDto.Id, bookEditDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenBookIdIsDifferentThenParameterId()
        {
            var bookEditDto = new BookEditDto() { Id = 1,  Name = "Test" };

            var result = await _booksController.Update(2, bookEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var bookEditDto = new BookEditDto() { Id = 1 };
            _booksController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _booksController.Update(1, bookEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromService_JustOnce()
        {
            var book = CreateBook();
            var bookEditDto = new BookEditDto() { Id = book.Id, Name = "Test" };

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookEditDto>())).Returns(book);
            _bookServiceMock.Setup(c => c.GetById(book.Id)).ReturnsAsync(book);
            _bookServiceMock.Setup(c => c.Update(book)).ReturnsAsync(book);

            await _booksController.Update(bookEditDto.Id, bookEditDto);

            _bookServiceMock.Verify(mock => mock.Update(book), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnOk_WhenBookIsRemoved()
        {
            var book = CreateBook();
            _bookServiceMock.Setup(c => c.GetById(book.Id)).ReturnsAsync(book);
            _bookServiceMock.Setup(c => c.Remove(book)).ReturnsAsync(true);

            var result = await _booksController.Remove(book.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenBookIdDoesNotExist()
        {
            var book = CreateBook();
            _bookServiceMock.Setup(c => c.GetById(book.Id)).ReturnsAsync((Book)null);

            var result = await _booksController.Remove(book.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromService_JustOnce()
        {
            var book = CreateBook();
            _bookServiceMock.Setup(c => c.GetById(book.Id)).ReturnsAsync(book);
            _bookServiceMock.Setup(c => c.Remove(book)).ReturnsAsync(true);

            await _booksController.Remove(book.Id);

            _bookServiceMock.Verify(mock => mock.Remove(book), Times.Once);
        }

        private Book CreateBook()
        {
            return new Book()
            {
                Id = 2,
                Name = "Book Test",
                Author = "Author Test",
                Description = "Description Test",
                Value = 10,
                CategoryId = 1,
                PublishDate = DateTime.MinValue.AddYears(40),
                Category = new Category()
                {
                    Id = 1,
                    Name = "Category Test"
                }
            };
        }

        private BookResultDto MapModelToBookResultDto(Book book)
        {
            var bookDto = new BookResultDto()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Description = book.Description,
                PublishDate = book.PublishDate,
                Value = book.Value,
                CategoryId = book.CategoryId
            };
            return bookDto;
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
                    Id = 1,
                    Name = "Book Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    Value = 20,
                    CategoryId = 1
                },
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    Value = 30,
                    CategoryId = 2
                }
            };
        }

        private List<BookResultDto> MapModelToBookResultListDto(List<Book> books)
        {
            var listBooks = new List<BookResultDto>();

            foreach (var item in books)
            {
                var book = new BookResultDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Author = item.Author,
                    Description = item.Description,
                    PublishDate = item.PublishDate,
                    Value = item.Value,
                    CategoryId = item.CategoryId
                };
                listBooks.Add(book);
            }
            return listBooks;
        }
    }
}