using static KedemMarket.Fairs.Services.FairConsts;

namespace KedemMarket.Fairs.Security;

public partial class FairConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new ("Admin area. Fairs view list", FairPermissions.ADMIN_VIEW, nameof(Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs create fair", FairPermissions.ADMIN_CREATE, nameof(Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs update fair", FairPermissions.ADMIN_EDIT, nameof(Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs delete fair", FairPermissions.ADMIN_DELETE, nameof(Fair), NopCustomerDefaults.AdministratorsRoleName),

        new ("Admin area. Fairs view list", FairPermissions.ADMIN_VIEW, nameof(Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs create fair", FairPermissions.ADMIN_CREATE, nameof(Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs update fair", FairPermissions.ADMIN_EDIT, nameof(Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs delete fair", FairPermissions.ADMIN_DELETE, nameof(Fair), CustomerRoleNames.FairManager),
        
        new ("Admin area. View vendor's fairs", FairPermissions.VENDOR_VIEW, nameof(Fair), NopCustomerDefaults.VendorsRoleName),
 };
}
