using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos.Category;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : MainController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(IMapper mapper,
                                    ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();

            return Ok(_mapper.Map<IEnumerable<CategoryResultDto>>(categories));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null) return NotFound();

            return Ok(_mapper.Map<CategoryResultDto>(category));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CategoryAddDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var category = _mapper.Map<Category>(categoryDto);
            var categoryResult = await _categoryService.Add(category);

            if (categoryResult == null) return BadRequest();

            return Ok(_mapper.Map<CategoryResultDto>(categoryResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, CategoryEditDto categoryDto)
        {
            if (id != categoryDto.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            await _categoryService.Update(_mapper.Map<Category>(categoryDto));

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null) return NotFound();

            var result = await _categoryService.Remove(category);

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Route("search/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Category>>> Search(string category)
        {
            var categories = _mapper.Map<List<Category>>(await _categoryService.Search(category));

            if (categories == null || categories.Count == 0)
                return NotFound("None category was founded");

            return Ok(categories);
        }
    }
}