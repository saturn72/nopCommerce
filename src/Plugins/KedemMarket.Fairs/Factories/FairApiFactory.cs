using KedemMarket.Common.Services.Media;
using KedemMarket.Fairs.Models;

namespace KedemMarket.Fairs.Factories;
public class FairApiFactory : IFairApiFactory
{
    private readonly MediaConvertor _mediaConvertor;

    public FairApiFactory(MediaConvertor mediaConvertor)
    {
        _mediaConvertor = mediaConvertor;
    }
    public async Task<FairListApiModel> PrepareFairApiModelListAsync(IEnumerable<Fair> fairs)
    {
        var fams = new List<FairApiModel>();
        foreach (var fair in fairs)
            fams.Add(await PrepareFairApiModelAsync(fair));

        return new()
        {
            Fairs = fams
        };
    }

    public Task<FairApiModel> PrepareFairApiModelAsync(Fair fair)
    {
        var fam = new FairApiModel
        {
            Id = fair.Id,
            Name = fair.Name,
            Description = fair.Description,
            StartsOnUtc = fair.StartsOnUtc,
            EndsOnUtc = fair.EndsOnUtc,
            //Tags = fair.Tags,
            //Image = await _mediaConvertor.ToGalleryItemModel(fair.Image),
            IsVirtual = fair.IsVirtual,
            IsFavorite = false, // This should be set based on the user's favorites
            //Url = fair.Url
        };
        return Task.FromResult(fam);
    }
}
