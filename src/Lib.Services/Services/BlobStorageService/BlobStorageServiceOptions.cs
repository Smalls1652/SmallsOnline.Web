namespace SmallsOnline.Web.Lib.Services;

public class BlobStorageServiceOptions
{
    /// <summary>
    /// The connection string for the Azure Storage Account.
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// The domain name to use for generated URLs.
    /// </summary>
    /// <remarks>
    /// Useful for utilizing a CDN or custom domain name for the storage account.
    /// </remarks>
    public string StorageAccountDomainName { get; set; } = null!;
}