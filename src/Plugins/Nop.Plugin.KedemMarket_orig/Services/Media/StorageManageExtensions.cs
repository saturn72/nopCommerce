<<<<<<< HEAD
﻿using Nop.Core.Domain.Media;
using SixLabors.ImageSharp;
=======
﻿using SixLabors.ImageSharp;
>>>>>>> dev/get-vendors-sales
using SixLabors.ImageSharp.Processing;
using static KedemMarket.KmConsts;

namespace KedemMarket.Services.Media;

public static class StorageManageExtensions
{
    public static async Task<string> CreateDownloadLink(this IStorageManager storageManager, string mediaType, int pictureId)
    {
        var webpPath = storageManager.GetWebpPath(mediaType, pictureId);
        return await storageManager.CreateDownloadLinkAsync(webpPath);
    }

<<<<<<< HEAD
    public static async Task UploadByKmMediaTypeAsync(this IStorageManager storageManager, string mediaType, PictureBinary pictureBinary)
=======
    public static async Task UploadByKmMediaTypeAsync(this IStorageManager storageManager, string mediaType, int pictureId, byte[] bytes)
>>>>>>> dev/get-vendors-sales
    {
        var resizeOptions = new Dictionary<string, ResizeOptions>
        {
            { MediaTypes.Thumbnail,
                new()
                {
                    Size = new()
                    {
                        Height = 0,
                        Width = 74,
                    },
                    Mode = ResizeMode.Min,
                    Sampler = KnownResamplers.Lanczos3,
                }
            },{
                MediaTypes.Image,
                new()
                {
                    Size = new()
                    {
                        Height = 0,
                        Width = 120,
                    },
                    Mode = ResizeMode.Min,
                    Sampler = KnownResamplers.Lanczos3,
                }
            }
        };
<<<<<<< HEAD
        using var inStream = new MemoryStream(pictureBinary.BinaryData);
=======
        using var inStream = new MemoryStream(bytes);
>>>>>>> dev/get-vendors-sales
        using var image = await Image.LoadAsync(inStream);
        using var outStream = new MemoryStream();
        {
            await image.SaveAsWebpAsync(outStream);

            image.Mutate(i => i.Resize(resizeOptions[mediaType]));
            var buffer = outStream.GetBuffer();
<<<<<<< HEAD
            var path = storageManager.GetWebpPath(mediaType, pictureBinary.PictureId);
=======
            var path = storageManager.GetWebpPath(mediaType, pictureId);
>>>>>>> dev/get-vendors-sales
            await storageManager.UploadAsync(path, "image/webp", buffer);
        }
    }
}