using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using System.Security.Claims;
using RealEstateApi.Models;
using RealEstateApi.DTOs;
using AutoMapper;
using RealEstateApi.Repositories;
using System.Threading.Tasks;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertyRepository propertyRepository, IMapper mapper, IUserRepository userRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet("PropertyList")]
        [Authorize]
        public async Task<IActionResult> GetProperties(int categoryId)
        {
            var proprtiesResult = await _propertyRepository.GetPropertiesAsync(categoryId);
                if(proprtiesResult == null)
            {
                return NotFound("No properties found");
            }
            return Ok(proprtiesResult);
        }

        [HttpGet("PropertyDetail")]
        [Authorize]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var propertyResult = await _propertyRepository.GetPropertyByIdAsync(id);
            if (propertyResult == null)
            {
                return NotFound($"Property with ID {id} not found");
            }
            var propertyDto = _mapper.Map<PropertyDto>(propertyResult);
            return Ok(propertyDto);
        }

        [HttpGet("TrendingProperties")]
        [Authorize]
        public async Task<IActionResult> GetTrendingProperties()
        {
            var trendingProperties =await _propertyRepository.GetTrendingProperties();
            if (trendingProperties == null)
            {
                return NotFound("No Trending Porperties Found");
            }
            return Ok(trendingProperties);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public async Task<IActionResult> SearchProperties(string query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            var searchResult = await _propertyRepository.SearchPropertiesAsync(query);
            if (!searchResult.Any())
            {
                return NotFound("No properties found");
            }

            return Ok(searchResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] PropertyDto propertyDto)
        {
            if (propertyDto == null)
            {
                return BadRequest("Property data is required.");
            }

            // Get the authenticated user's email from JWT token
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("Invalid User");
            }
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return Unauthorized("User Not found");

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

           await _propertyRepository.AddPropertyAsync(property);
            await _propertyRepository.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, property);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id,[FromBody] PropertyDto propertyDto)
        {
            var propertyResult = await _propertyRepository.GetPropertyByIdAsync(id);
            if (propertyResult == null)
            {
                return NotFound($"Property with ID {id} not found");
            }

            // Get the authenticated user's email from JWT token
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("You are not authorized to update this property");
            }
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null || propertyResult.UserId != user.Id)
            {
                return Unauthorized("You are not authorized to update this property");
            }

            if (propertyResult.UserId != user.Id)
            {
                return Forbid("You are not allowed to update this property.");
            }
            bool categoryExists = await _propertyRepository.CategoryExistsAsync(propertyDto.CategoryId);
            if (!categoryExists) { 
                return BadRequest("Category does not exist");
            }

            propertyResult.Name = propertyDto.Name;
                propertyResult.Detail = propertyDto.Details;
                propertyResult.Address = propertyDto.Address;
                propertyResult.Price = (int)propertyDto.Price;
                propertyResult.ImageUrl = propertyDto.ImageUrl;
                propertyResult.CategoryId = propertyDto.CategoryId;
            

            await _propertyRepository.UpdatePropertyAsync(propertyResult);
            await _propertyRepository.SaveChangesAsync();
                
            return Ok("Record Successfully Updated");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var propertyResult = await _propertyRepository.GetPropertyByIdAsync(id);
            if (propertyResult == null)
            {
                return NotFound($"Property with ID {id} not found");
            }
            // Get the authenticated user's email from JWT token
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("You are not authorized to delete this property");
            }

            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null || propertyResult.UserId != user.Id)
            {
                return Unauthorized("User not found");
            }
            if(propertyResult.UserId != user.Id)
            {
                return Forbid("You are not allowed to delete this property.");
            }

            await _propertyRepository.DeletePropertyAsync(id);
            await _propertyRepository.SaveChangesAsync();
            return Ok("Record Successfully Deleted");
        }

    }
}
