using static KedemMarket.Fair.Services.FairConsts;

namespace KedemMarket.Fair.Security;

public partial class FairConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new ("Admin area. Fairs view list", FairPermissions.FAIRS_VIEW, nameof(FairInfo), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs create fair", FairPermissions.FAIRS_CREATE, nameof(FairInfo), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs update fair", FairPermissions.FAIRS_EDIT, nameof(FairInfo), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs delete fair", FairPermissions.FAIRS_DELETE, nameof(FairInfo), NopCustomerDefaults.AdministratorsRoleName),

        new ("Admin area. Fairs view list", FairPermissions.FAIRS_VIEW, nameof(FairInfo), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs create fair", FairPermissions.FAIRS_CREATE, nameof(FairInfo), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs update fair", FairPermissions.FAIRS_EDIT, nameof(FairInfo), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs delete fair", FairPermissions.FAIRS_DELETE, nameof(FairInfo), CustomerRoleNames.FairManager),

        //new ("Admin area. Fair elements view fair",FairPermissions.FAIRS_ELEMENTS_VIEW, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements create fair",FairPermissions.FAIRS_ELEMENTS_CREATE, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements update fair",FairPermissions.FAIRS_ELEMENTS_EDIT, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements delete fair",FairPermissions.FAIRS_ELEMENTS_DELETE, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
    };
}
