using System.Globalization;

namespace Nadeko.Medusa;

public interface IMedusaLoaderService
{
    Task<bool> LoadSnekAsync(string medusaName);
    Task<bool> UnloadSnekAsync(string medusaName);
    string GetCommandDescription(string medusaName, string commandName, CultureInfo culture);
    string[] GetCommandUsages(string medusaName, string commandName, CultureInfo culture);
    Task ReloadStrings();
    IReadOnlyCollection<string> GetAvailableMedusae();
    IReadOnlyCollection<string> GetLoadedMedusae();
}