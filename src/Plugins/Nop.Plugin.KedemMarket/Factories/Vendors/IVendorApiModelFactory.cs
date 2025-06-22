
namespace KedemMarket.Factories.Vendors;
public interface IVendorApiModelFactory
{
    Task<IList<VendorApiModel>> GetAllVendorsAsync();
    Task<VendorApiModel> PrepareVendorApiModelAsync(Vendor vendor);
}
