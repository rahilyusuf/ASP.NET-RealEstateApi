using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public CategoriesController(ApiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            var categories = _context.Categories.Select(c => new
            {
                c.Id,
                c.Name,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{c.ImageUrl}"
            }).ToList();
            return Ok(categories);
        }
    }
}
