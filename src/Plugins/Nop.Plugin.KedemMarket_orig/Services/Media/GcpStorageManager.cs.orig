using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
<<<<<<< HEAD
using Nop.Services.Logging;
=======
>>>>>>> dev/get-vendors-sales

namespace KedemMarket.Services.Media;
public class GcpStorageManager : IStorageManager
{
<<<<<<< HEAD
    private readonly ILogger _logger;
=======
>>>>>>> dev/get-vendors-sales
    private readonly IOptionsMonitor<GcpOptions> _options;
    private readonly string[] _scopes = new[]
       {
       "https://www.googleapis.com/auth/cloud-platform",
       "https://www.googleapis.com/auth/firebase",
    };
    private readonly StorageClient _storageClient;

    public GcpStorageManager(
<<<<<<< HEAD
        IOptionsMonitor<GcpOptions> options,
        ILogger logger)
    {
        _options = options;
        _logger = logger;
=======
        IOptionsMonitor<GcpOptions> options)
    {
        _options = options;
>>>>>>> dev/get-vendors-sales

        var cred = GoogleCredential.GetApplicationDefault();
        _ = cred.CreateScoped(_scopes)
      .UnderlyingCredential;

        _storageClient = StorageClient.Create();
    }

    public Task DeleteAsync(string path)
    {
        var p = PreparePath(path);
        return _storageClient.DeleteObjectAsync(_options.CurrentValue.BucketName, path);
    }

    public async Task UploadAsync(string path, string contentType, byte[] bytes)
    {
        var p = PreparePath(path);

        using var stream = new MemoryStream(bytes);
        _ = await _storageClient.UploadObjectAsync(_options.CurrentValue.BucketName, p, contentType, stream);
    }

    public async Task<string> CreateDownloadLinkAsync(string webpPath)
    {
        var p = PreparePath(webpPath);
        var urlSigner = _storageClient.CreateUrlSigner();

        return await urlSigner.SignAsync(_options.CurrentValue.BucketName, p, TimeSpan.FromDays(7), HttpMethod.Get);
    }

    private string PreparePath(string path)
    {
        //remove heading slashes
        while (path.StartsWith('/'))
            path = path.Substring(1);
        path = path.Replace("  ", " ").Replace(' ', '-').ToLower();
        return path;
    }

    public string GetWebpPath(string mediaType, int pictureId) => $"/{mediaType}/{pictureId}.webp";
}
