using System.Collections.Generic;
using System.Linq;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Infrastructure.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<BookStoreDbContext> _options;

        /// <summary>
        /// The CategoryRepository class only use the methods from the Repository class
        /// </summary>
        public CategoryRepositoryTests()
        {
            // Use this when using a SQLite InMemory database
            _options = BookStoreHelperTests.BookStoreDbContextOptionsSQLiteInMemory();
            BookStoreHelperTests.CreateDataBaseSQLiteInMemory(_options);

            // Use this when using a EF Core InMemory database
            //_options = BookStoreHelperTests.BookStoreDbContextOptionsEfCoreInMemory();
            //BookStoreHelperTests.CreateDataBaseEfCoreInMemory(_options);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfCategories_WhenCategoriesExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);

                var categories = await categoryRepository.GetAll();

                Assert.NotNull(categories);
                Assert.IsType<List<Category>>(categories);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAnEmptyList_WhenCategoriesDoNotExist()
        {
            await BookStoreHelperTests.CleanDataBase(_options);

            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                var categories = await categoryRepository.GetAll();

                Assert.NotNull(categories);
                Assert.Empty(categories);
                Assert.IsType<List<Category>>(categories);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfCategoriesWithCorrectValues_WhenCategoriesExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var expectedCategories = CreateCategoryList();
                var categoryRepository = new CategoryRepository(context);
                var categoryList = await categoryRepository.GetAll();

                Assert.Equal(3, categoryList.Count);
                Assert.Equal(expectedCategories[0].Id, categoryList[0].Id);
                Assert.Equal(expectedCategories[0].Name, categoryList[0].Name);
                Assert.Equal(expectedCategories[1].Id, categoryList[1].Id);
                Assert.Equal(expectedCategories[1].Name, categoryList[1].Name);
                Assert.Equal(expectedCategories[2].Id, categoryList[2].Id);
                Assert.Equal(expectedCategories[2].Name, categoryList[2].Name);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnCategoryWithSearchedId_WhenCategoryWithSearchedIdExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                var category = await categoryRepository.GetById(2);

                Assert.NotNull(category);
                Assert.IsType<Category>(category);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnCategoryWithCorrectValues_WhenCategoryExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);

                var expectedCategories = CreateCategoryList();
                var category = await categoryRepository.GetById(2);

                Assert.Equal(expectedCategories[1].Id, category.Id);
                Assert.Equal(expectedCategories[1].Name, category.Name);
            }
        }

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenCategoryIsValid()
        {
            Category categoryToRemove = new Category();

            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                categoryToRemove = await categoryRepository.GetById(2);
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);

                await categoryRepository.Remove(categoryToRemove);
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRemoved = await context.Categories.Where(c => c.Id == 2).FirstOrDefaultAsync();

                Assert.Null(categoryRemoved);
            }
        }

        [Fact]
        public async void UpdateCategory_ShouldUpdateCategoryWithCorrectValues_WhenCategoryIsValid()
        {
            Category categoryToUpdate = new Category();
            await using (var context = new BookStoreDbContext(_options))
            {
                categoryToUpdate = await context.Categories.Where(b => b.Id == 1).FirstOrDefaultAsync();
                categoryToUpdate.Name = "Updated Name";
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await categoryRepository.Update(categoryToUpdate);
            }

            await using (var context = new BookStoreDbContext(_options))
            {
                var updatedCategory = await context.Categories.Where(b => b.Id == 1).FirstOrDefaultAsync();

                Assert.NotNull(updatedCategory);
                Assert.IsType<Category>(updatedCategory);
                Assert.Equal(categoryToUpdate.Id, updatedCategory.Id);
                Assert.Equal(categoryToUpdate.Name, updatedCategory.Name);
            }
        }

        private List<Category> CreateCategoryList()
        {
            return new List<Category>()
            {
                new Category {Id = 1, Name = "Category 1"},
                new Category {Id = 2, Name = "Category 2"},
                new Category {Id = 3, Name = "Category 3"}
            };
        }
    }
}