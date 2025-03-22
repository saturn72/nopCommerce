namespace KedemMarket.Fair.Security;
public class FairPermissions
{
    private const string FairInfoPrefix = "FairInfo";

    public const string FAIRS_VIEW = FairInfoPrefix+".FairsView";
    public const string FAIRS_CREATE = FairInfoPrefix+".FairsCreate";
    public const string FAIRS_EDIT = FairInfoPrefix+".FairsEdit";
    public const string FAIRS_DELETE = FairInfoPrefix+".FairsDelete";
}