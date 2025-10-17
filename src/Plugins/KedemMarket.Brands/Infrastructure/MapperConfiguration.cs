using AutoMapper;
using Nop.Core.Infrastructure.Mapper;

namespace KedemMarket.Brands.Infrastructure;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<Brand, BrandModel>()
            .ReverseMap();
    }
    public int Order => 10;
}