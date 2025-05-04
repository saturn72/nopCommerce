using KedemMarket.Common.Models.Media;
using KedemMarket.Common.Services.Media;
using KedemMarket.Fairs.Models;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Media;

namespace KedemMarket.Fairs.Factories;
public class FairApiFactory : IFairApiFactory
{
    private readonly MediaConvertor _mediaConvertor;
    private readonly IWorkContext _workContext;
    private readonly IRepository<FairCustomerFavoriteMap> _fairCustomerFavoriteMapRepository;
    private readonly IRepository<Vendor> _vendorRepository;
    private readonly IFairService _fairService;
    private readonly IPictureService _pictureService;
    private readonly IProductService _productService;

    public FairApiFactory(
        MediaConvertor mediaConvertor,
        IWorkContext workContext,
        IRepository<FairCustomerFavoriteMap> fairCustomerFavoriteMapRepository,
        IRepository<Vendor> vendorRepository,
        IFairService fairService,
        IPictureService pictureService,
        IProductService productService)
    {
        _mediaConvertor = mediaConvertor;
        _workContext = workContext;
        _fairCustomerFavoriteMapRepository = fairCustomerFavoriteMapRepository;
        _vendorRepository = vendorRepository;
        _fairService = fairService;
        _pictureService = pictureService;
        _productService = productService;
    }
    public async Task<FairListApiModel> PrepareFairApiModelListAsync(IEnumerable<Fair> fairs)
    {
        var fairIds = fairs.Select(x => x.Id).ToList();
        var favs = await (from f in _fairCustomerFavoriteMapRepository.Table
                          where fairIds.Contains(f.FairId)
                          select f).ToListAsync();

        var favIds = favs.Select(x => x.FairId).ToList();
        var fams = new List<FairApiModel>();

        foreach (var fair in fairs)
        {
            var vendors = await _fairService.GetVendorsByFairIdAsync(fair.Id);
            var fam = await BuildCustomerFairApiModel(fair, favIds.Contains(fair.Id), vendors);
            fams.Add(fam);
        }

        return new()
        {
            Fairs = fams
        };
    }

    public async Task<FairApiModel> PrepareFairApiModelAsync(Fair fair, IEnumerable<Vendor> vendors)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var fav = await _fairCustomerFavoriteMapRepository.Table.FirstOrDefaultAsync(f => f.CustomerId == customer.Id && f.FairId == fair.Id);
        var fam = await BuildCustomerFairApiModel(fair, isFavorite: fav != null, vendors);

        return fam;
    }

    public async Task<FairVendorApiModel> PrepareFairVendorApiModelAsync(Fair fair, Vendor vendor)
    {
        var pic = await _pictureService.GetPictureByIdAsync(vendor.PictureId);
        var vi = await _mediaConvertor.ToGalleryItemModelAsync(pic, 0);
        var maps = await _fairService.GetFairVendorProductMapsAsync(fair, vendor);

        var prodctIds = maps?.Select(map => map.ProductId).ToArray() ?? [];
        var vendorProducts = await _productService.GetProductsByIdsAsync(prodctIds);

        var products = await vendorProducts.SelectAwait(async vp => await ToFaiVendorProductApiModelAsync(vp)).ToListAsync();

        return new FairVendorApiModel
        {
            Id = vendor.Id,
            Name = vendor.Name,
            Description = vendor.Description,
            Image = vi,
            Products = products
            //Url = v.Url
        };
    }

    private async Task<FairVendorProductApiModel> ToFaiVendorProductApiModelAsync(Product product)
    {

        var gmi = default(GalleryItemModel);
        var allProductPictures = await _productService.GetProductPicturesByProductIdAsync(product.Id);
        var productPicture = allProductPictures?.MinBy(x => x.DisplayOrder);
        if (productPicture != null)
        {
            var picture = await _pictureService.GetPictureByIdAsync(productPicture.PictureId);
            gmi = await _mediaConvertor.ToGalleryItemModelAsync(picture, 0);
        }

        return new FairVendorProductApiModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.FullDescription,
            Picture = gmi,
        };
    }

    private async Task<FairApiModel> BuildCustomerFairApiModel(Fair fair, bool isFavorite, IEnumerable<Vendor> vendors)
    {
        var vs = new List<FairVendorApiModel>();
        foreach (var v in vendors)
        {
            var fvam = await PrepareFairVendorApiModelAsync(fair, v);
            vs.Add(fvam);
        }

        GalleryItemModel image = null;
        var fairPicture = await _pictureService.GetPictureByIdAsync(fair.PictureId);
        if (fairPicture != null)
            image = await _mediaConvertor.ToGalleryItemModelAsync(fairPicture, 0);

        return new()
        {
            Id = fair.Id,
            Name = fair.Name,
            Description = fair.Description,
            StartsOnLocalDateTime = fair.StartsOnLocalDateTime,
            EndsOnLocalDateTime = fair.EndsOnLocalDateTime,
            //Tags = fair.Tags,
            Image = image,
            IsVirtual = fair.IsVirtual,
            IsFavorite = isFavorite,
            //Url = fair.Url
            Vendors = vs,
        };
    }
}
