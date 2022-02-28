public interface ISnekLoaderService
{
    public Task<bool> LoadSnekAsync(string name);
    public Task<bool> UnloadSnekAsync(string name);
}