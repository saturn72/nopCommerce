namespace KedemMarket.Brands.Security;

public partial class BrandsPermissionConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new ("Admin area. Brands view list", BrandsPermissions.BRANDS_VIEW, nameof(Brand), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Brands create brand", BrandsPermissions.BRANDS_CREATE_EDIT_DELETE, nameof(Brand), NopCustomerDefaults.AdministratorsRoleName),
    };
}
