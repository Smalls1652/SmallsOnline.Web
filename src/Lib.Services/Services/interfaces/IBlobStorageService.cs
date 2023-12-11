namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Interface for the Azure Storage Account Blob service.
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Upload an image to the storage account.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="data">The image's data.</param>
    /// <returns>The URL for the uploaded image.</returns>
    Task<string> UploadImageAsync(string fileName, Stream data);

    Task<string> UploadBlogImageAsync(string fileName, string mimeType, Stream data);
}