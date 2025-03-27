namespace KedemMarket.Fairs.Security;
public class FairPermissions
{
    private const string FairPrefix = "Fair";

    public const string VIEW = FairPrefix + ".FairsView";
    public const string CREATE = FairPrefix + ".FairsCreate";
    public const string EDIT = FairPrefix + ".FairsEdit";
    public const string DELETE = FairPrefix + ".FairsDelete";
}