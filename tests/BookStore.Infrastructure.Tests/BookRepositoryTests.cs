using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Infrastructure.Tests
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<BookStoreDbContext> _options;

        public BookRepositoryTests()
        {
            // Use this when using a SQLite InMemory database
            _options = BookStoreHelperTests.BookStoreDbContextOptionsSQLiteInMemory();
            BookStoreHelperTests.CreateDataBaseSQLiteInMemory(_options);

            // Use this when using a EF Core InMemory database
            //_options = BookStoreHelperTests.BookStoreDbContextOptionsEfCoreInMemory();
            //BookStoreHelperTests.CreateDataBaseEfCoreInMemory(_options);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfBook_WhenBooksExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.GetAll();

                Assert.NotNull(books);
                Assert.IsType<List<Book>>(books);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAnEmptyList_WhenBooksDoNotExist()
        {
            await BookStoreHelperTests.CleanDataBase(_options);

            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.GetAll();

                Assert.NotNull(books);
                Assert.Empty(books);
                Assert.IsType<List<Book>>(books);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfBooksWithCorrectValues_WhenBooksExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);

                var expectedBooks = CreateBookList();
                var bookList = await bookRepository.GetAll();

                Assert.Equal(3, bookList.Count);
                Assert.Equal(expectedBooks[0].Id, bookList[0].Id);
                Assert.Equal(expectedBooks[0].Name, bookList[0].Name);
                Assert.Equal(expectedBooks[0].Description, bookList[0].Description);
                Assert.Equal(expectedBooks[0].CategoryId, bookList[0].CategoryId);
                Assert.Equal(expectedBooks[0].PublishDate, bookList[0].PublishDate);
                Assert.Equal(expectedBooks[0].Value, bookList[0].Value);
                Assert.Equal(expectedBooks[0].Category.Id, bookList[0].Category.Id);
                Assert.Equal(expectedBooks[0].Category.Name, bookList[0].Category.Name);
                Assert.Equal(expectedBooks[1].Id, bookList[1].Id);
                Assert.Equal(expectedBooks[1].Name, bookList[1].Name);
                Assert.Equal(expectedBooks[1].Description, bookList[1].Description);
                Assert.Equal(expectedBooks[1].CategoryId, bookList[1].CategoryId);
                Assert.Equal(expectedBooks[1].PublishDate, bookList[1].PublishDate);
                Assert.Equal(expectedBooks[1].Value, bookList[1].Value);
                Assert.Equal(expectedBooks[1].Category.Id, bookList[1].Category.Id);
                Assert.Equal(expectedBooks[1].Category.Name, bookList[1].Category.Name);
                Assert.Equal(expectedBooks[2].Id, bookList[2].Id);
                Assert.Equal(expectedBooks[2].Name, bookList[2].Name);
                Assert.Equal(expectedBooks[2].Description, bookList[2].Description);
                Assert.Equal(expectedBooks[2].CategoryId, bookList[2].CategoryId);
                Assert.Equal(expectedBooks[2].PublishDate, bookList[2].PublishDate);
                Assert.Equal(expectedBooks[2].Value, bookList[2].Value);
                Assert.Equal(expectedBooks[2].Category.Id, bookList[2].Category.Id);
                Assert.Equal(expectedBooks[2].Category.Name, bookList[2].Category.Name);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnBookWithSearchedId_WhenBookWithSearchedIdExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                
                var book = await bookRepository.GetById(2);

                Assert.NotNull(book);
                Assert.IsType<Book>(book);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnBookWithCorrectValues_WhenBookExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                
                var expectedBooks = CreateBookList();
                var book = await bookRepository.GetById(2);

                Assert.Equal(expectedBooks[1].Id, book.Id);
                Assert.Equal(expectedBooks[1].Name, book.Name);
                Assert.Equal(expectedBooks[1].Description, book.Description);
                Assert.Equal(expectedBooks[1].CategoryId, book.CategoryId);
                Assert.Equal(expectedBooks[1].PublishDate, book.PublishDate);
                Assert.Equal(expectedBooks[1].Value, book.Value);
                Assert.Equal(expectedBooks[1].Category.Id, book.Category.Id);
                Assert.Equal(expectedBooks[1].Category.Name, book.Category.Name);
            }
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnNull_WhenNoBooksWithSearchedIdExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);

                var book = await bookRepository.GetById(4);

                Assert.Null(book);
            }
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnListOfBooks_WhenBooksWithSearchedCategoryExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);

                var books = await bookRepository.GetBooksByCategory(1);

                Assert.NotNull(books);
                Assert.IsType<List<Book>>(books);
            }
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnListOfBooksWithSearchedCategory_WhenBooksWithSearchedCategoryExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);

                var expectedBooks = CreateBookList();
                var books = await bookRepository.GetBooksByCategory(1);
                var bookList = books as List<Book>;

                Assert.NotNull(bookList);
                Assert.Equal(2, bookList.Count);

                Assert.Equal(expectedBooks[0].Id, bookList[0].Id);
                Assert.Equal(expectedBooks[0].Name, bookList[0].Name);
                Assert.Equal(expectedBooks[0].Description, bookList[0].Description);
                Assert.Equal(expectedBooks[0].CategoryId, bookList[0].CategoryId);
                Assert.Equal(expectedBooks[0].PublishDate, bookList[0].PublishDate);
                Assert.Equal(expectedBooks[0].Value, bookList[0].Value);
                Assert.Equal(expectedBooks[1].Id, bookList[1].Id);
                Assert.Equal(expectedBooks[1].Name, bookList[1].Name);
                Assert.Equal(expectedBooks[1].Description, bookList[1].Description);
                Assert.Equal(expectedBooks[1].CategoryId, bookList[1].CategoryId);
                Assert.Equal(expectedBooks[1].PublishDate, bookList[1].PublishDate);
                Assert.Equal(expectedBooks[1].Value, bookList[1].Value);
            }
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnAnEmptyList_WhenNoBooksWithSearchedCategoryExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.GetBooksByCategory(4);
                var bookList = books as List<Book>;

                Assert.NotNull(bookList);
                Assert.Empty(bookList);
                Assert.IsType<List<Book>>(bookList);
            }
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnOneBook_WhenOneBookWithSearchedValueExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var expectedBook = CreateBookList();
                var books = await bookRepository.SearchBookWithCategory(expectedBook[1].Name);
                var bookList = books as List<Book>;

                Assert.NotNull(bookList);
                Assert.IsType<List<Book>>(bookList);
                Assert.Single(bookList);
                Assert.Equal(expectedBook[1].Id, bookList[0].Id);
                Assert.Equal(expectedBook[1].Name, bookList[0].Name);
                Assert.Equal(expectedBook[1].Author, bookList[0].Author);
                Assert.Equal(expectedBook[1].Description, bookList[0].Description);
                Assert.Equal(expectedBook[1].CategoryId, bookList[0].CategoryId);
                Assert.Equal(expectedBook[1].PublishDate, bookList[0].PublishDate);
                Assert.Equal(expectedBook[1].Value, bookList[0].Value);
            }
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnListOfBooks_WhenBookWithSearchedValueExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var expectedBooks = CreateBookList();
                var books = await bookRepository.SearchBookWithCategory("Book Test");
                var bookList = books as List<Book>;

                Assert.NotNull(bookList);
                Assert.IsType<List<Book>>(bookList);
                Assert.Equal(expectedBooks.Count, bookList.Count);
                Assert.Equal(expectedBooks[0].Id, bookList[0].Id);
                Assert.Equal(expectedBooks[0].Name, bookList[0].Name);
                Assert.Equal(expectedBooks[0].Description, bookList[0].Description);
                Assert.Equal(expectedBooks[0].CategoryId, bookList[0].CategoryId);
                Assert.Equal(expectedBooks[0].PublishDate, bookList[0].PublishDate);
                Assert.Equal(expectedBooks[0].Value, bookList[0].Value);
                Assert.Equal(expectedBooks[0].Category.Id, bookList[0].Category.Id);
                Assert.Equal(expectedBooks[0].Category.Name, bookList[0].Category.Name);
                Assert.Equal(expectedBooks[1].Id, bookList[1].Id);
                Assert.Equal(expectedBooks[1].Name, bookList[1].Name);
                Assert.Equal(expectedBooks[1].Description, bookList[1].Description);
                Assert.Equal(expectedBooks[1].CategoryId, bookList[1].CategoryId);
                Assert.Equal(expectedBooks[1].PublishDate, bookList[1].PublishDate);
                Assert.Equal(expectedBooks[1].Value, bookList[1].Value);
                Assert.Equal(expectedBooks[1].Category.Id, bookList[1].Category.Id);
                Assert.Equal(expectedBooks[1].Category.Name, bookList[1].Category.Name);
                Assert.Equal(expectedBooks[2].Id, bookList[2].Id);
                Assert.Equal(expectedBooks[2].Name, bookList[2].Name);
                Assert.Equal(expectedBooks[2].Description, bookList[2].Description);
                Assert.Equal(expectedBooks[2].CategoryId, bookList[2].CategoryId);
                Assert.Equal(expectedBooks[2].PublishDate, bookList[2].PublishDate);
                Assert.Equal(expectedBooks[2].Value, bookList[2].Value);
                Assert.Equal(expectedBooks[2].Category.Id, bookList[2].Category.Id);
                Assert.Equal(expectedBooks[2].Category.Name, bookList[2].Category.Name);
            }
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnAnEmptyList_WhenNoBooksWithSearchedValueExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.SearchBookWithCategory("Testt");
                var bookList = books as List<Book>;

                Assert.NotNull(bookList);
                Assert.Empty(bookList);
                Assert.IsType<List<Book>>(bookList);
            }
        }

        [Fact]
        public async void AddBook_ShouldAddBookWithCorrectValues_BookIsValid()
        {
            Book bookToAdd = new Book();

            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                bookToAdd = CreateBook();

                await bookRepository.Add(bookToAdd);
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var bookResult = await context.Books.Include(b => b.Category).Where(b => b.Id == 4).FirstOrDefaultAsync();

                Assert.NotNull(bookResult);
                Assert.IsType<Book>(bookResult);
                Assert.Equal(bookToAdd.Id, bookResult.Id);
                Assert.Equal(bookToAdd.Name, bookResult.Name);
                Assert.Equal(bookToAdd.Description, bookResult.Description);
                Assert.Equal(bookToAdd.CategoryId, bookResult.CategoryId);
                Assert.Equal(bookToAdd.PublishDate, bookResult.PublishDate);
                Assert.Equal(bookToAdd.Value, bookResult.Value);
            }
        }

        [Fact]
        public async void UpdateBook_ShouldUpdateBookWithCorrectValues_WhenBookIsValid()
        {
            Book bookToUpdate = new Book();
            await using (var context = new BookStoreDbContext(_options))
            {
                bookToUpdate = await context.Books.Where(b => b.Id == 1).FirstOrDefaultAsync();
                bookToUpdate.Name = "Updated Name";
                bookToUpdate.Description = "Updated Description";
                bookToUpdate.Author = "Updated Author";
                bookToUpdate.PublishDate = new DateTime(2019, 4, 4, 0, 0, 0, 0);
                bookToUpdate.Value = 100;
                bookToUpdate.CategoryId = 2;
                bookToUpdate.Category = new Category()
                {
                    Id = 2,
                    Name = "Category 2"
                };
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                await bookRepository.Update(bookToUpdate);
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var updatedBook = await context.Books.Include(b => b.Category).Where(b => b.Id == 1).FirstOrDefaultAsync();

                Assert.NotNull(updatedBook);
                Assert.IsType<Book>(updatedBook);
                Assert.Equal(bookToUpdate.Id, updatedBook.Id);
                Assert.Equal(bookToUpdate.Name, updatedBook.Name);
                Assert.Equal(bookToUpdate.Description, updatedBook.Description);
                Assert.Equal(bookToUpdate.CategoryId, updatedBook.CategoryId);
                Assert.Equal(bookToUpdate.PublishDate, updatedBook.PublishDate);
                Assert.Equal(bookToUpdate.Value, updatedBook.Value);
                Assert.Equal(bookToUpdate.Category.Id, updatedBook.Category.Id);
                Assert.Equal(bookToUpdate.Category.Name, updatedBook.Category.Name);
            }
        }

        private Book CreateBook()
        {
            return new Book()
            {
                Id = 4,
                Name = "Book Test 4",
                Author = "Author Test 4",
                Description = "Description Test 4",
                Value = 40,
                CategoryId = 2,
                PublishDate = new DateTime(2020, 4, 4, 0, 0, 0, 0)
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
                    CategoryId = 1,
                    PublishDate = new DateTime(2020, 1, 1, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 1,
                        Name = "Category 1"
                    }
                },
                new Book()
                {
                    Id = 2,
                    Name = "Book Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    Value = 20,
                    CategoryId = 1,
                    PublishDate = new DateTime(2020, 2, 2, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 1,
                        Name = "Category 1"
                    }
                },
                new Book()
                {
                    Id = 3,
                    Name = "Book Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    Value = 30,
                    CategoryId = 3,
                    PublishDate = new DateTime(2020, 3, 3, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 3,
                        Name = "Category 3"
                    }
                }
            };
        }
    }
}