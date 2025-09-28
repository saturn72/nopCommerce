using Nop.Web.Models.Media;

namespace KedemMarket.Services.Media;
public interface IMediaManager
{
    public Task DeleteAsync(string? mediaType, int mediaId);
    public Task<string?> GetDownloadLinkAsync(int mediaItemId, string? mediaType);
    public Task<GalleryItemModel?> ToGalleryItemModel(Picture? picture, int index);
    public Task<GalleryItemModel?> ToGalleryItemModelAsync(PictureModel? picture, int index);
    public GalleryItemModel? ToGalleryItemModel(Video? video, int index);
    public Task UploadByMediaTypeAsync(string? mediaType, int pictureId, byte[]? pictureBinary);
}