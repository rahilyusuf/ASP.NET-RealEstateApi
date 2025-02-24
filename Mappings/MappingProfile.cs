using AutoMapper;
using RealEstateApi.DTOs;
using RealEstateApi.Models;

namespace RealEstateApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            // Map Property -> PropertyDto (for GET requests)
            CreateMap<Property, PropertyDto>();
            // Map PropertyDto -> Property (for POST/PUT requests)
            CreateMap<PropertyDto, Property>();

        }
    }
}
