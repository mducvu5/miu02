using System.Globalization;

public interface IMedusaLoaderService
{
    Task<bool> LoadSnekAsync(string name);
    Task<bool> UnloadSnekAsync(string name);
    string GetCommandDescription(string medusaName, string commandName, CultureInfo culture);
    string[] GetCommandUsages(string medusaName, string commandName, CultureInfo culture);
}