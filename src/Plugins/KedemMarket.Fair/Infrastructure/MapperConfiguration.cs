using AutoMapper;

namespace KedemMarket.Fair.Infrastructure;
public  class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<FairInfo, FairInfoAdminModel>()
              .ReverseMap();
    }
    public int Order => 10;
}