using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Service for interacting with an Azure Storage Account's blobs.
/// </summary>
public partial class BlobStorageService : IBlobStorageService
{
    /// <summary>
    /// The underlying <see cref="BlobContainerClient"/> for the service.
    /// </summary>
    private readonly BlobContainerClient _blobContainerClient;

    /// <summary>
    /// The domain name for the storage account.
    /// </summary>
    /// <remarks>
    /// Useful for utilizing a CDN or custom domain name for the storage account.
    /// </remarks>
    private readonly string _storageAccountDomainName;

    /// <summary>
    /// Create a new instance of the <see cref="BlobStorageService"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string for the Azure Storage Account.</param>
    /// <param name="storageAccountDomainName">The domain name to use for generated URLs.</param>
    public BlobStorageService(string connectionString, string storageAccountDomainName)
    {
        _blobContainerClient = new(
            connectionString: connectionString,
            blobContainerName: "images"
        );

        _storageAccountDomainName = storageAccountDomainName;
    }

    /// <summary>
    /// Create a new instance of the <see cref="BlobStorageService"/> class.
    /// </summary>
    /// <param name="options">The <see cref="BlobStorageServiceOptions"/> for configuring the service.</param>
    public BlobStorageService(IOptions<BlobStorageServiceOptions> options)
    {
        _blobContainerClient = new(
            connectionString: options.Value.ConnectionString,
            blobContainerName: "images"
        );

        _storageAccountDomainName = options.Value.StorageAccountDomainName;
    }

    /// <inheritdoc />
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