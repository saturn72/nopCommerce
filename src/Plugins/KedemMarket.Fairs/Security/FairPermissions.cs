namespace KedemMarket.Fairs.Security;
public class FairPermissions
{
    private const string FairPrefix = "Fair";

    public const string ADMIN_VIEW = FairPrefix + ".Admin.FairsView";
    public const string ADMIN_CREATE = FairPrefix + ".Admin.FairsCreate";
    public const string ADMIN_EDIT = FairPrefix + ".Admin.FairsEdit";
    public const string ADMIN_DELETE = FairPrefix + ".Admin.FairsDelete";

    public const string VENDOR_VIEW = FairPrefix + ".Vendor.FairsView";
}