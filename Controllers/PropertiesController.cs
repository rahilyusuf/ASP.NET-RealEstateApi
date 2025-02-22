using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using System.Security.Claims;
using RealEstateApi.Models;
using RealEstateApi.DTOs;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PropertiesController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] PropertyDto propertyDto)
        {
            if (propertyDto == null)
            {
                return BadRequest("Property data is required.");
            }

            // Get the authenticated user's email from JWT token
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound("User not found");
            }

      
            var property = new Property
            {
                Name = propertyDto.Name,
                Detail=propertyDto.Details,
                Address = propertyDto.Address,
                Price = (int)propertyDto.Price,
                ImageUrl = propertyDto.ImageUrl,
                CategoryId = propertyDto.CategoryId,
                IsTrending = false, // Default false
                UserId = user.Id // Assign user who created it
            };

            _context.Properties.Add(property);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, property);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id,[FromBody] PropertyDto propertyDto)
        {
            var propertyResult = _context.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null)
            {
                return NotFound($"Property with ID {id} not found");
            }

            // Get the authenticated user's email from JWT token
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null || propertyResult.UserId != user.Id)
            {
                return Unauthorized("You are not authorized to update this property");
            }


           
                propertyResult.Name = propertyDto.Name;
                propertyResult.Detail = propertyDto.Details;
                propertyResult.Address = propertyDto.Address;
                propertyResult.Price = (int)propertyDto.Price;
                propertyResult.ImageUrl = propertyDto.ImageUrl;
                propertyResult.CategoryId = propertyDto.CategoryId;
            

         
            _context.SaveChanges();

            return NoContent();
        }
    }
}
