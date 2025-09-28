
namespace KedemMarket.Consumers;

public class MediaEventsConsumer :

    IConsumer<EntityInsertedEvent<Category>>,
    IConsumer<EntityUpdatedEvent<Category>>,
    IConsumer<EntityDeletedEvent<Category>>,

    IConsumer<EntityInsertedEvent<Manufacturer>>,
    IConsumer<EntityUpdatedEvent<Manufacturer>>,
    IConsumer<EntityDeletedEvent<Manufacturer>>,

     IConsumer<EntityInsertedEvent<PictureBinary>>,
     IConsumer<EntityUpdatedEvent<PictureBinary>>,
     IConsumer<EntityDeletedEvent<PictureBinary>>,

    IConsumer<EntityInsertedEvent<ProductAttributeCombinationPicture>>,
    IConsumer<EntityUpdatedEvent<ProductAttributeCombinationPicture>>,
    IConsumer<EntityDeletedEvent<ProductAttributeCombinationPicture>>,

    IConsumer<EntityInsertedEvent<ProductPicture>>,
    IConsumer<EntityUpdatedEvent<ProductPicture>>,
    IConsumer<EntityDeletedEvent<ProductPicture>>,

    //IConsumer<EntityInsertedEvent<ProductVideo>>,
    //IConsumer<EntityUpdatedEvent<ProductVideo>>,
    //IConsumer<EntityDeletedEvent<ProductVideo>>,

    IConsumer<EntityInsertedEvent<Vendor>>,
    IConsumer<EntityUpdatedEvent<Vendor>>,
    IConsumer<EntityDeletedEvent<Vendor>>
{
    private readonly IMediaManager _mediaConvertor;
    private readonly IPictureService _pictureService;

    public MediaEventsConsumer(
        IMediaManager mediaconvertor,
        IPictureService pictureService)
    {
        _mediaConvertor = mediaconvertor;
        _pictureService = pictureService;
    }

    public async Task HandleEventAsync(EntityInsertedEvent<Category> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<Category> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<Category> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }
    private async Task UploadToStorageByPictureIdAsync(int pictureId, PictureBinary? pictureBinary)
    {
        if (pictureId == 0)
            return;

        var paths = GetStorageImagePaths(pictureId);
        pictureBinary ??= await _pictureService.GetPictureBinaryByPictureIdAsync(pictureId);

        var tasks = new[]{
            KmConsts.MediaTypes.Thumbnail,
            KmConsts.MediaTypes.Image
        }.Select(mt => _mediaConvertor.UploadByMediaTypeAsync(mt, pictureBinary.PictureId, pictureBinary.BinaryData)).ToArray();

        await Task.WhenAll(tasks);
    }

    private async Task DeleteAllImagesFromStorageByPictureIdAsync(int pictureId)
    {
        if (pictureId == 0)
            return;
        var paths = await GetStorageImagePaths(pictureId);
        var tasks = paths.Select(_mediaConvertor.DeleteAsync).ToArray();
        await Task.WhenAll(tasks);
    }

    private async Task<string[]> GetStorageImagePaths(int pictureId)
    {
        return await Task.WhenAll(
           _mediaConvertor.GetThumbnailDownloadLink(pictureId),
           _mediaConvertor.GetImageDownloadLink(pictureId)
            );
    }

    public async Task HandleEventAsync(EntityInsertedEvent<Manufacturer> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<Manufacturer> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<Manufacturer> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }

    public async Task HandleEventAsync(EntityInsertedEvent<PictureBinary> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, eventMessage.Entity);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<PictureBinary> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, eventMessage.Entity);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<PictureBinary> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }

    public async Task HandleEventAsync(EntityInsertedEvent<ProductAttributeCombinationPicture> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<ProductAttributeCombinationPicture> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<ProductAttributeCombinationPicture> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }

    public async Task HandleEventAsync(EntityInsertedEvent<ProductPicture> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<ProductPicture> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<ProductPicture> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }


    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage)
    {
        await UploadToStorageByPictureIdAsync(eventMessage.Entity.PictureId, null);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<Vendor> eventMessage)
    {
        await DeleteAllImagesFromStorageByPictureIdAsync(eventMessage.Entity.PictureId);
    }
}
