using AutoMapper;

namespace KedemMarket.Fairs.Infrastructure;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<Fair, FairAdminModel>()
            .ForMember(dest => dest.Address, mo => mo.MapFrom(src => src.IsVirtual ? src.ToModel<AddressModel>() : null));

        CreateMap<FairAdminModel, Fair>()
            .ForMember(dest => dest.Address, mo => mo.MapFrom(src => src.IsVirtual ? src.ToEntity<Address>() : null));

        CreateMap<CreateOrUpdateFairVendorModel, FairVendorMap>();
    }
    public int Order => 10;
}