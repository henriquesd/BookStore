using System.Collections.Generic;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Moq;
using Xunit;

namespace BookStore.Domain.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IBookService> _bookService;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _bookService = new Mock<IBookService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _bookService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfCategories_WhenCategoriesExist()
        {
            var categories = CreateCategoryList();

            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync(categories);

            var result = await _categoryService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenCategoriesDoNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((List<Category>)null);

            var result = await _categoryService.GetAll();

            Assert.Null(result);
        }
        
        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((List<Category>)null);

            await _categoryService.GetAll();

            _categoryRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnCategory_WhenCategoryExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.GetById(category.Id)).ReturnsAsync(category);

            var result = await _categoryService.GetById(category.Id);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Category)null);

            var result = await _categoryService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Category)null);

            await _categoryService.GetById(1);

            _categoryRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.Name == category.Name))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.Add(category));

            var result = await _categoryService.Add(category);
            
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddCategory_WhenCategoryNameAlreadyExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>() { category };

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.Name == category.Name)).ReturnsAsync(categoryList);

            var result = await _categoryService.Add(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.Name == category.Name))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.Add(category));

            await _categoryService.Add(category);

            _categoryRepositoryMock.Verify(mock => mock.Add(category), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.Update(category));

            var result = await _categoryService.Update(category);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }
        
        [Fact]
        public async void Update_ShouldNotUpdateCategory_WhenCategoryDoesNotExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>()
            {
                new Category()
                {
                    Id = 2,
                    Name = "Category Name 2"
                }
            };

            _categoryRepositoryMock.Setup(c => 
                    c.Search(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(categoryList);

            var result = await _categoryService.Update(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(new List<Category>());

            await _categoryService.Update(category);

            _categoryRepositoryMock.Verify(mock => mock.Update(category), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenCategoryDoNotHaveRelatedBooks()
        {
            var category = CreateCategory();

            _bookService.Setup(b =>
                b.GetBooksByCategory(category.Id)).ReturnsAsync(new List<Book>());

            var result = await _categoryService.Remove(category);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldNotRemoveCategory_WhenCategoryHasRelatedBooks()
        {
            var category = CreateCategory();

            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Name = "Test Name 1",
                    Author = "Test Author 1",
                    CategoryId = category.Id
                }
            };

            _bookService.Setup(b => b.GetBooksByCategory(category.Id)).ReturnsAsync(books);

            var result = await _categoryService.Remove(category);

            Assert.False(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _bookService.Setup(b =>
                b.GetBooksByCategory(category.Id)).ReturnsAsync(new List<Book>());

            await _categoryService.Remove(category);

            _categoryRepositoryMock.Verify(mock => mock.Remove(category), Times.Once);
        }


        [Fact]
        public async void Search_ShouldReturnAListOfCategory_WhenCategoriesWithSearchedNameExist()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.Name.Contains(categoryName)))
                .ReturnsAsync(categoryList);

                var result = await _categoryService.Search(searchedCategory.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenCategoriesWithSearchedNameDoNotExist()
        {
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.Name.Contains(categoryName)))
                .ReturnsAsync((IEnumerable<Category>)(null));

            var result = await _categoryService.Search(searchedCategory.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.Name.Contains(categoryName)))
                .ReturnsAsync(categoryList);

            await _categoryService.Search(searchedCategory.Name);

            _categoryRepositoryMock.Verify(mock => mock.Search(c => c.Name.Contains(categoryName)), Times.Once);
        }

        private Category CreateCategory()
        {
            return new Category()
            {
                Id = 1,
                Name = "Category Name 1"
            };
        }

        private List<Category> CreateCategoryList()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Category Name 1"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Category Name 2"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Category Name 3"
                }
            };
        }
    }
}