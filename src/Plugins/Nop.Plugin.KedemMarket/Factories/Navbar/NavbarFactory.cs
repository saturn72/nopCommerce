
namespace KedemMarket.Factories.Navbar;

public class NavbarFactory : INavbarFactory
{
    private readonly INavbarService _navbarService;
    private readonly IVendorService _vendorService;
    private readonly IMediaManager _mediaConverter;
    private readonly IPictureService _pictureService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IGenericAttributeService _genericAttributeService;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    private IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    private readonly IProductService _productService;
    private readonly IProductApiFactory _productApiFactory;

    public NavbarFactory(
        INavbarService navbarService,
        IVendorService vendorService,
        IMediaManager mediaConverter,
        IPictureService pictureService,
        IStaticCacheManager staticCacheManager,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        IGenericAttributeService genericAttributeService,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IProductService productService,
        IProductApiFactory productApiFactory)
    {
        _navbarService = navbarService;
        _vendorService = vendorService;
        _mediaConverter = mediaConverter;
        _pictureService = pictureService;
        _staticCacheManager = staticCacheManager;
        _vendorAttributeService = vendorAttributeService;
        _genericAttributeService = genericAttributeService;
        _vendorAttributeParser = vendorAttributeParser;
        _productService = productService;
        _productApiFactory = productApiFactory;
    }

    public async Task<NavbarModel> PrepareNavbarModelByNameAsync(string name)
    {
        var key = new CacheKey($"{NavbarCacheSettings.CACHE_KEY}.{name}", NavbarCacheSettings.CACHE_KEY)
        {
            CacheTime = NavbarCacheSettings.CACHE_TIME
        };

        var nam = await _staticCacheManager.GetAsync<NavbarModel>(key);
        if (nam != null)
            return nam;

        var navbar = await _navbarService.GetNavbarInfoByNameAsync(name);
        if (navbar == null)
            return null;

        var elms = navbar?.Elements ?? [];
        var elements = new List<Models.Navbar.NavbarElementModel>();

        var allVendorAttributes = await _vendorAttributeService.GetAllAttributesAsync();
        var shortDescriptionAttribute = allVendorAttributes.FirstOrDefault(va => va.Name == KmConsts.VendorAttributeNames.ShortDescription);
        var whatsappAttribute = allVendorAttributes.FirstOrDefault(va => va.Name == KmConsts.VendorAttributeNames.Whatsapp);
        var phoneAttribute = allVendorAttributes.FirstOrDefault(va => va.Name == KmConsts.VendorAttributeNames.Phone);

        foreach (var e in elms)
        {
            var nevs = await _navbarService.GetNavbarElementVendorsByNavbarElementIdAsync(e.Id);
            var vendors = await nevs.Where(x => x.Published)
                .SelectAwait(async nev => await _vendorService.GetVendorByIdAsync(nev.VendorId))
                .ToListAsync();

            var gitTemp = new Dictionary<int, Task<GalleryItemModel>>();
            var vpTemp = new Dictionary<int, Task<IPagedList<Product>>>();
            foreach (var v in vendors)
            {
                var gitTask = async () =>
                {
                    if (v.PictureId <= 0)
                        return null;

                    var pic = await _pictureService.GetPictureByIdAsync(v.PictureId);
                    return await _mediaConverter.ToGalleryItemModel(pic, 0);
                };
                gitTemp[v.Id] = gitTask();
                vpTemp[v.Id] = _productService.SearchProductsAsync(vendorId: v.Id);
            }
            await Task.WhenAll(gitTemp.Values);
            await Task.WhenAll(vpTemp.Values);

            var vendorModels = new List<NavbarVendorModel>();
            foreach (var v in vendors)
            {
                var selectedVendorAttributes = await _genericAttributeService.GetAttributeAsync<string>(v, NopVendorDefaults.VendorAttributes);

                var shortDescription = GetAttributeValueOrNull(selectedVendorAttributes, shortDescriptionAttribute);

                _ = gitTemp.TryGetValue(v.Id, out var pic);

                var productSlims = await _productApiFactory.ToProductSlimApiModelAsync(await vpTemp[v.Id]);
                var navbarVendor = nevs.First(nev => nev.VendorId == v.Id);
                var whatsapp = navbarVendor.PublishWhatsapp ? GetAttributeValueOrNull(selectedVendorAttributes, whatsappAttribute) : null;
                var phone = navbarVendor.PublishPhone ? GetAttributeValueOrNull(selectedVendorAttributes, phoneAttribute) : null;

                vendorModels.Add(new()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Phone = phone,
                    Picture = pic?.Result,
                    Products = productSlims,
                    ShortDescription = shortDescription,
                    Whatsapp = whatsapp,
                });
            }

            //var vendors 
            var ne = new Models.Navbar.NavbarElementModel
            {
                ActiveIcon = e.ActiveIcon,
                Alt = e.Alt,
                Caption = e.Caption,
                Icon = e.Icon,
                Index = e.Index,
                Tags = e.Tags,
                Type = e.Type,
                Value = e.Value,
                Vendors = vendorModels,
            };
            elements.Add(ne);
        }

        nam = new NavbarModel
        {
            Elements = elements,
        };
        await _staticCacheManager.SetAsync(key, nam);

        return nam;
    }

    private string? GetAttributeValueOrNull(string attributesXml, VendorAttribute? attribute)
    {
        return attribute != null ?
            _vendorAttributeParser.ParseValues(attributesXml, attribute.Id).FirstOrDefault() :
            null;
    }
}