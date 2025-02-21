using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealEstateApi.Data;
using RealEstateApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RealEstateApi.DTOs; 


namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _config;

        public UserController(ApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost("[action]")]
        public IActionResult Register([FromBody] UserRegisterRequest user)
        {
            var userExists = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if(userExists != null)
            {
                return BadRequest("User already exists");
            }
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            var CurrentUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (CurrentUser == null)
            {
                return NotFound("User not found");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email)
                

            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
             
        }
    }
}
