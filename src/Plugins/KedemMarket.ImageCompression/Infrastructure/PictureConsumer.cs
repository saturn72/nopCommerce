using Nop.Core.Domain.Media;
using Nop.Core.Events;
using Nop.Data;
using Nop.Services.Events;
using SkiaSharp;

namespace KedemMarket.ImageCompression.Infrastructure;

/// <summary>
/// Represents the object for the configuring services on application startup
/// </summary>
public class PictureConsumer :
    IConsumer<EntityInsertedEvent<PictureBinary>>,
    IConsumer<EntityUpdatedEvent<PictureBinary>>
{
    private const string WEBP_MIME_TYPE = "webp";

    private readonly IRepository<Picture> _pictureRepository;
    private readonly IRepository<PictureBinary> _pictureBinaryRepository;

    public PictureConsumer(
        IRepository<Picture> pictureRepository,
        IRepository<PictureBinary> pictureBinaryRepository
        )
    {
        _pictureRepository = pictureRepository;
        _pictureBinaryRepository = pictureBinaryRepository;
    }

    public async Task HandleEventAsync(EntityInsertedEvent<PictureBinary> eventMessage) => 
        await HandleEventAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<PictureBinary> eventMessage) =>
        await HandleEventAsync(eventMessage.Entity);

    private async Task HandleEventAsync(PictureBinary? pictureBinary)
    {
        var picture = await _pictureRepository.GetByIdAsync(pictureBinary?.PictureId);
        if (picture == null)
            return;

        var parts = picture.MimeType.ToLowerInvariant().Split('/');
        var lastPart = parts[^1];
        var isWebP = lastPart != WEBP_MIME_TYPE;
        if (!isWebP)
            return;

        var bytes = (await _pictureBinaryRepository.Table
            .FirstOrDefaultAsync(pb => pb.PictureId == picture.Id))?.BinaryData;
        if (bytes == null || bytes.Length == 0)
            return;

        try
        {

            var newBytes = ProcessAndCompressData(bytes);
            if (newBytes == null)
                return;

            picture.MimeType = WEBP_MIME_TYPE;
            await _pictureRepository.UpdateAsync(picture);
            await _pictureBinaryRepository.UpdateAsync(new PictureBinary
            {
                BinaryData = newBytes,
                PictureId = picture.Id,
            });
        }
        catch { }
    }
    private byte[]? ProcessAndCompressData(byte[] bytes)
    {
        using var data = SKData.CreateCopy(bytes);
        using var image = SKImage.FromEncodedData(data);
        if (image == null)
            return null;
        using var encodedData = image.Encode(SKEncodedImageFormat.Webp, 100);
        return encodedData?.ToArray();
    }
}