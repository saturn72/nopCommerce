using static KedemMarket.KmConsts;

namespace KedemMarket.Services.Media;

public static class MediaConvertorExtensions
{
    public async static Task<string> GetThumbnailDownloadLink(this IMediaManager convertor, int pictureId)
    {
        return await convertor.GetDownloadLinkAsync(pictureId, MediaTypes.Thumbnail);
    }

    public async static Task<string> GetImageDownloadLink(this IMediaManager convertor, int pictureId)
    {
        return await convertor.GetDownloadLinkAsync(pictureId, MediaTypes.Image);
    }

    public async static Task<string> GetVideoDownloadLink(this IMediaManager convertor, int videoId)
    {
        return await convertor.GetDownloadLinkAsync(videoId, MediaTypes.Video);
    }
}
