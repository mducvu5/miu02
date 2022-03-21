#nullable disable
using LinqToDB.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NadekoBot.Services.Database;

namespace NadekoBot.Services;

public class DbService
{
    private readonly DbContextOptions<NadekoSqliteContext> _options;
    private readonly DbContextOptions<NadekoSqliteContext> _migrateOptions;
    private readonly IBotCredentials _creds;

    public DbService(IBotCredentials creds)
    {
        LinqToDBForEFTools.Initialize();
        _creds = creds;
    }

    public async Task Setup()
    {
        await using var context = new NadekoSqliteContext(_creds.Db.ConnectionString);
        var migrations = await context.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            // wait indefinitely for migrations
            // as they might take a very long time
            await using var mContext = new NadekoSqliteContext(_creds.Db.ConnectionString, 0);
            await mContext.Database.MigrateAsync();
            await mContext.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL");
        await context.SaveChangesAsync();
    }

    private NadekoSqliteContext GetDbContextInternal()
    {
        var dbType = _creds.Db.Type;
        var connString = _creds.Db.ConnectionString;
        
        var context = new NadekoSqliteContext(connString);
        var conn = context.Database.GetDbConnection();
        conn.Open();
        using var com = conn.CreateCommand();
        com.CommandText = "PRAGMA synchronous=OFF";
        com.ExecuteNonQuery();
        return context;
    }

    public NadekoSqliteContext GetDbContext()
        => GetDbContextInternal();
}