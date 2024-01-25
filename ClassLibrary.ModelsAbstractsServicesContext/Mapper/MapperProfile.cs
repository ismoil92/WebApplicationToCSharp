using AutoMapper;
using ClassLibrary.ModelsAbstractsServicesContext.Models;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
namespace ClassLibrary.ModelsAbstractsServicesContext.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Storage, StorageDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}