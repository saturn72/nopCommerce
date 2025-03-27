using static KedemMarket.Fairs.Services.FairConsts;

namespace KedemMarket.Fairs.Security;

public partial class FairConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new ("Admin area. Fairs view list", FairPermissions.VIEW, nameof(Domain.Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs create fair", FairPermissions.CREATE, nameof(Domain.Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs update fair", FairPermissions.EDIT, nameof(Domain.Fair), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Fairs delete fair", FairPermissions.DELETE, nameof(Domain.Fair), NopCustomerDefaults.AdministratorsRoleName),

        new ("Admin area. Fairs view list", FairPermissions.VIEW, nameof(Domain.Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs create fair", FairPermissions.CREATE, nameof(Domain.Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs update fair", FairPermissions.EDIT, nameof(Domain.Fair), CustomerRoleNames.FairManager),
        new ("Admin area. Fairs delete fair", FairPermissions.DELETE, nameof(Domain.Fair), CustomerRoleNames.FairManager),

        //new ("Admin area. Fair elements view fair",FairPermissions.FAIRS_ELEMENTS_VIEW, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements create fair",FairPermissions.FAIRS_ELEMENTS_CREATE, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements update fair",FairPermissions.FAIRS_ELEMENTS_EDIT, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
        //new ("Admin area. Fair elements delete fair",FairPermissions.FAIRS_ELEMENTS_DELETE, nameof(FairElement), NopCustomerDefaults.AdministratorsRoleName),
    };
}
