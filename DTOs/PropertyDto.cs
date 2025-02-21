using System.ComponentModel.DataAnnotations;

namespace RealEstateApi.DTOs
{
    public class PropertyDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
