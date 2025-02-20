using System.ComponentModel.DataAnnotations;

namespace RealEstateApi.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }
    }
}
