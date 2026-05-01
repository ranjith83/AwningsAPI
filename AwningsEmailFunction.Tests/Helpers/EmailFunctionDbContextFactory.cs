using Microsoft.EntityFrameworkCore;

namespace AwningsEmailFunction.Tests.Helpers;

public static class EmailFunctionDbContextFactory
{
    public static EmailFunctionDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<EmailFunctionDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;
        return new EmailFunctionDbContext(options);
    }
}
