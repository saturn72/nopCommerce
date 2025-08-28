using Nop.Web.Models.Media;
using static KedemMarket.KmConsts;

namespace KedemMarket.Services.Media;
public sealed class MediaManager : IMediaManager
{
    private readonly IStorageManager _storageManager;
    private readonly IStaticCacheManager _staticCache;

    public MediaManager(
        IStorageManager storageManager,
        IStaticCacheManager staticCache)
    {
        _storageManager = storageManager;
        _staticCache = staticCache;
    }
    public async Task DeleteAsync(string mediaType, int mediaId)
    {
        var path = _storageManager.GetWebpPath(mediaType, mediaId);
        await _storageManager.DeleteAsync(path);
        _ = DeleteFromCacheInternal(path);
    }
    private async Task DeleteFromCacheInternal(string path)
    {
        var key = new CacheKey(path);
        await _staticCache.RemoveAsync(key);
    }

    public async Task<string> GetDownloadLinkAsync(int mediaItemId, string mediaType)
    {
        var path = _storageManager.GetWebpPath(mediaType, mediaItemId);
        var key = new CacheKey(path)
        {
            CacheTime = (int)TimeSpan.FromDays(7).Subtract(TimeSpan.FromMinutes(30)).TotalMinutes,
        };

        return await _staticCache.GetAsync(key, async () => await _storageManager.CreateDownloadLinkAsync(path));
    }
    public async Task UploadByMediaTypeAsync(string mediaType, int pictureId, byte[] pictureBinary)
    {
        await _storageManager.UploadByKmMediaTypeAsync(mediaType, pictureId, pictureBinary);
        var path = _storageManager.GetWebpPath(mediaType, pictureId);
        await DeleteFromCacheInternal(path);
        _ = GetDownloadLinkAsync(pictureId, mediaType);
    }

    public async Task<GalleryItemModel> ToGalleryItemModel(Picture picture, int index)
    {
        picture.ThrowArgumentNullException(nameof(picture));

        return new()
        {
            Alt = picture.AltAttribute,
            FullImage = await GetDownloadLinkAsync(picture.Id, MediaTypes.Image),
            Index = index,
            ThumbImage = await GetDownloadLinkAsync(picture.Id, MediaTypes.Thumbnail),
            Title = picture.TitleAttribute,
            Type = "image"
        };
    }

    public async Task<GalleryItemModel> ToGalleryItemModelAsync(PictureModel picture, int index)
    {
        if (picture == null)
            return null;

        return new()
        {
            Alt = picture.AlternateText,
            FullImage = await GetDownloadLinkAsync(picture.Id, MediaTypes.Image),
            Index = index,
            ThumbImage = await GetDownloadLinkAsync(picture.Id, MediaTypes.Thumbnail),
            Title = picture.Title,
            Type = "image"
        };
    }


    public GalleryItemModel ToGalleryItemModel(Video video, int index)
    {
        if (video == null)
            return null;

        return new()
        {
            Index = index,
            Type = "video",
            Url = video.VideoUrl,
        };
    }
}
