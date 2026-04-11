using AutoMapper;
using Nop.Core.Infrastructure.Mapper;

namespace KedemMarket.CMS.Infrastructure;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
    }
    public int Order => 1;
}