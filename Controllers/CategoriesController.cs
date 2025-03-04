using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using RealEstateApi.DTOs;
using RealEstateApi.Repositories;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {

            var categories = await _categoryRepository.GetCategoriesAsync();
            if(categories == null || !categories.Any())
            {
                return NotFound();
            }
            var CategoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{c.ImageUrl}"
            }).ToList();
            return Ok(CategoryDTOs);
        }
    }
}
