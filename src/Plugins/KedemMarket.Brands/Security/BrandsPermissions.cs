namespace KedemMarket.Brands.Security;
public class BrandsPermissions
{
    public const string BRANDS_VIEW = $"{nameof(Brand)}.BrandsView";
    public const string BRANDS_CREATE_EDIT_DELETE = $"{nameof(Brand)}.BrandCreateEditDelete";
}