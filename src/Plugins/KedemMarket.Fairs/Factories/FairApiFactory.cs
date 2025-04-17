using KedemMarket.Common.Models.Media;
using KedemMarket.Common.Services.Media;
using KedemMarket.Fairs.Models;
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

    public FairApiFactory(
        MediaConvertor mediaConvertor,
        IWorkContext workContext,
        IRepository<FairCustomerFavoriteMap> fairCustomerFavoriteMapRepository,
        IRepository<Vendor> vendorRepository,
        IFairService fairService,
        IPictureService pictureService)
    {
        _mediaConvertor = mediaConvertor;
        _workContext = workContext;
        _fairCustomerFavoriteMapRepository = fairCustomerFavoriteMapRepository;
        _vendorRepository = vendorRepository;
        _fairService = fairService;
        _pictureService = pictureService;
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


    private async Task<FairApiModel> BuildCustomerFairApiModel(Fair fair, bool isFavorite, IEnumerable<Vendor> vendors)
    {
        var vs = vendors?.Select(v => new FairVendorApiModel
        {
            Id = v.Id,
            Name = v.Name,
            Description = v.Description,
            //Image = await _mediaConvertor.ToGalleryItemModel(v.Picture),
            //Url = v.Url
        }).ToList();

        GalleryItemModel image = null;
        var fairPicture = await _pictureService.GetPictureByIdAsync(fair.PictureId);
        if (fairPicture != null)
            image = await _mediaConvertor.ToGalleryItemModelAsync(fairPicture, 0);

        return new()
        {
            Id = fair.Id,
            Name = fair.Name,
            Description = fair.Description,
            StartsOnUtc = fair.StartsOnUtc,
            EndsOnUtc = fair.EndsOnUtc,
            //Tags = fair.Tags,
            Image = image,
            IsVirtual = fair.IsVirtual,
            IsFavorite = isFavorite,
            //Url = fair.Url
            Vendors = vs ?? [],
        };
    }
}
