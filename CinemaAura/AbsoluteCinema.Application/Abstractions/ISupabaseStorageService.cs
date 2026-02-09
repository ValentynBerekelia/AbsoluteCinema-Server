public interface IStorageService
{
    Task DeleteFileAsync(string relativePath);
    Task<string> UploadImageAsync(Stream fileStream, string fileName);
}