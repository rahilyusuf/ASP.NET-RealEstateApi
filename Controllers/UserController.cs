
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using RealEstateApi.Data;
using RealEstateApi.Models;

using RealEstateApi.DTOs;
using Microsoft.AspNetCore.Identity;
using RealEstateApi.Services;


namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthService _authService;

        public UserController(ApiDbContext context, IConfiguration config, IPasswordHasher<User> passwordHasher, IAuthService authService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        [HttpGet("[action]")]
        public IActionResult HasExistingPasswords()
        {
            var users = _context.Users.ToList();

            foreach (var u in users)
            {
                if (!IsHashed(u.Password))
                {
                    u.Password = _passwordHasher.HashPassword(u, u.Password);
                }
            }
            _context.SaveChanges();
            return Ok("Password successfully updated");
        }
        private bool IsHashed(string password)
        {
            return password.Length == 88 && password[0] == 'A' && password[1] == 'Q';
        }


        [HttpPost("[action]")]
        public IActionResult Register([FromBody] UserRegisterRequest user)
        {
            var userExists = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userExists != null)
            {
                return BadRequest("User already exists");
            }
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = _passwordHasher.HashPassword(new User(), user.Password),
                Phone = user.Phone
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var userResponse = new UserResponse
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Phone = newUser.Phone
            };
            return Created("User Created", userResponse);
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            var CurrentUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (CurrentUser == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(CurrentUser, CurrentUser.Password, user.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid password");
            }

            var jwt = _authService.GenerateJwtToken(CurrentUser.Email);
            return Ok(new AuthResponseDTO { Token = jwt, Email = CurrentUser.Email });
        }
    }
}
