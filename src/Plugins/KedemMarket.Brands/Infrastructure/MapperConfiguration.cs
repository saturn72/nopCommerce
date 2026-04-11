using AutoMapper;
using KedemMarket.Brands.Admin.Models;
using Nop.Core.Infrastructure.Mapper;

namespace KedemMarket.Brands.Infrastructure;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<Brand, BrandProductAdminModel>()
            .ReverseMap();
    }
    public int Order => 10;
}