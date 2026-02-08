using Microsoft.Extensions.Configuration;

namespace AbsoluteCinema.Infrastructure.Storage;

public class SupabaseStorageService : IStorageService
{
    private readonly string _url;
    private readonly string _key;
    private readonly string _bucket;

    public SupabaseStorageService(IConfiguration configuration)
    {
        _url = configuration["Supabase:Url"] ?? string.Empty;
        _key = configuration["Supabase:ApiKey"] ?? string.Empty;
        _bucket = configuration["Supabase:BucketName"] ?? string.Empty;
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
    {
        var options = new Supabase.SupabaseOptions { AutoConnectRealtime = false };
        var client = new Supabase.Client(_url, _key, options);
        await client.InitializeAsync();

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);

        await client.Storage.From(_bucket).Upload(ms.ToArray(), fileName);

        return client.Storage.From(_bucket).GetPublicUrl(fileName);
    }

}