public interface IStorageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName);
}