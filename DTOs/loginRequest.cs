using System.ComponentModel.DataAnnotations;

namespace RealEstateApi.DTOs
{
    public class loginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
