using APICatalogo.DTOs;
using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTO.Mapping;

public class ProductDTOMappingProfile : Profile
{
    public ProductDTOMappingProfile()
    {
        CreateMap<ProductDTO, ProductDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();    
    }
}
