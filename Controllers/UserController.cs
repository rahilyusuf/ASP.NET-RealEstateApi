
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using RealEstateApi.Data;
using RealEstateApi.Models;

using RealEstateApi.DTOs;
using Microsoft.AspNetCore.Identity;
using RealEstateApi.Services;
using RealEstateApi.Repositories;


namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthService _authService;

        public UserController(IUserRepository userRepository, IConfiguration config, IPasswordHasher<User> passwordHasher, IAuthService authService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest user)
        {
            if (await _userRepository.UserExistsAsync(user.Email))
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

            var userResponse = new UserResponse
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Phone = newUser.Phone
            };
            await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return Created("User Created", userResponse);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            var CurrentUser = await _userRepository.GetUserByEmailAsync(user.Email);
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
