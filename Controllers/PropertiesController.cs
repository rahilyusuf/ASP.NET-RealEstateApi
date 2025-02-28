using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using System.Security.Claims;
using RealEstateApi.Models;
using RealEstateApi.DTOs;
using AutoMapper;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public PropertiesController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("PropertyList")]
        [Authorize]
        public IActionResult GetProperties(int categoryId)
        {
            var proprtiesResult = _context.Properties.Where(p => p.CategoryId == categoryId);
                if(proprtiesResult == null)
            {
                return NotFound("No properties found");
            }
            return Ok(proprtiesResult);
        }

        [HttpGet("PropertyDetail")]
        [Authorize]
        public IActionResult GetPropertyDetail(int id)
        {
            var propertyResult = _context.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null)
            {
                return NotFound($"Property with ID {id} not found");
            }
            var propertyDto = _mapper.Map<PropertyDto>(propertyResult);
            return Ok(propertyDto);
        }

        [HttpGet("TrendingProperties")]
        [Authorize]
        public IActionResult GetTrendingProperties()
        {
            var trendingProperties = _context.Properties.Where(p => p.IsTrending == true);
            if (trendingProperties == null)
            {
                return NotFound();
            }
            return Ok(trendingProperties);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult SearchProperties(string query)
        {
            Console.WriteLine($"Received Query: {query}");

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            var searchResult = _context.Properties.Where(p => p.Name.Contains(query) || p.Address.Contains(query)).ToList(); ;
            
            if (!searchResult.Any()) 
            {
                return NotFound("No properties found");
            }

            return Ok(searchResult);
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
                
            return Ok("Record Successfully Updated");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
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
                return Unauthorized("You are not authorized to delete this property");
            }
            _context.Properties.Remove(propertyResult);
            _context.SaveChanges();
            return Ok("Record Successfully Deleted");
        }

    }
}
