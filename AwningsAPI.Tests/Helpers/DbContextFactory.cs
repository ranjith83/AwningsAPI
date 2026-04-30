using AwningsAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Tests.Helpers;

/// <summary>
/// Creates isolated in-memory AppDbContext instances for unit tests.
/// Each call with no argument gets a unique database so tests never share state.
/// </summary>
public static class DbContextFactory
{
    public static AppDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
