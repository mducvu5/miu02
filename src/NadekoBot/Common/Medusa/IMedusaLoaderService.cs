public interface IMedusaLoaderService
{
    Task<bool> LoadSnekAsync(string name);
    Task<bool> UnloadSnekAsync(string name);
}