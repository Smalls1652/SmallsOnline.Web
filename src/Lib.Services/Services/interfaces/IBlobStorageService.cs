namespace SmallsOnline.Web.Lib.Services;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(string fileName, Stream data);
}