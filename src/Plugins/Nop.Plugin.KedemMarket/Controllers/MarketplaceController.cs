namespace KedemMarket.Controllers;

[Route("api/marketplace")]
public class MarketplaceController : KmApiControllerBase
{
    private readonly IStoreContext _storeContext;
    private readonly IDirectoryFactory _directoryFactory;
    private readonly IWorkContext _workContext;

    public MarketplaceController(
        IStoreContext storeContext,
        IDirectoryFactory directoryFactory,
        IWorkContext workContext)
    {
        _storeContext = storeContext;
        _directoryFactory = directoryFactory;
        _workContext = workContext;
        _directoryFactory = directoryFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetStoreInfoByStoreIdAsync()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        var store = await _storeContext.GetCurrentStoreAsync();
        if (store == default)
            return BadRequest();

        var phone = _directoryFactory.ProcessPhoneNumber(store.CompanyPhoneNumber);
        var data = new StoreInfoApiModel
        {
            StoreName = store.Name,
            DisplayName = store.DefaultTitle,
            Phone = phone,
            Url = store.Url,
            SocialLinks = new Dictionary<string, string>
            {
                { KmConsts.SocialLinkNames.Facebook , "https://www.facebook.com/KedemMarket.co.il" },
                { KmConsts.SocialLinkNames.Instagram , "https://www.instagram.com/kedemmarket.co.il/"},
                { KmConsts.SocialLinkNames.Linktr , "https://linktr.ee/kedemmarket" },
            }
        };
        return ToJsonResult(data);
    }
}
