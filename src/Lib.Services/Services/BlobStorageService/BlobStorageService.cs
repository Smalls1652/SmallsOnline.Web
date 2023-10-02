using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SmallsOnline.Web.Lib.Services;

public partial class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly string _storageAccountDomainName;

    public BlobStorageService(string connectionString, string storageAccountDomainName)
    {
        _blobContainerClient = new(
            connectionString: connectionString,
            blobContainerName: "images"
        );

        _storageAccountDomainName = storageAccountDomainName;
    }

    public async Task<string> UploadImageAsync(string fileName, Stream data)
    {
        await _blobContainerClient.DeleteBlobIfExistsAsync($"top-music/uploaded/{fileName}");

        BlobContentInfo blobInfo = await _blobContainerClient.UploadBlobAsync(
            blobName: $"top-music/uploaded/{fileName}",
            content: data
        );

        BlobClient blobClient = _blobContainerClient.GetBlobClient($"top-music/uploaded/{fileName}");

        BlobHttpHeaders blobHttpHeaders = new()
        {
            ContentType = "image/jpeg"
        };

        await blobClient.SetHttpHeadersAsync(
            httpHeaders: blobHttpHeaders
        );

        return $"https://{_storageAccountDomainName}/{blobClient.BlobContainerName}/top-music/uploaded/{fileName}";
    }
}