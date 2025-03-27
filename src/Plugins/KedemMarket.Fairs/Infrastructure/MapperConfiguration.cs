using AutoMapper;

namespace KedemMarket.Fairs.Infrastructure;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<Fair, FairAdminModel>();
        CreateMap<CreateOrUpdateFairVendorModel, FairVendorMap>();
    }
    public int Order => 10;
}