using System.Data;
using Dapper;

namespace GameStore.Api.Users.Infrastructure.Persistence.Seeding;

public static class UsersSeed
{
    public static async Task SeedTestData(IDbConnection connection)
    {
        var schema = await File.ReadAllTextAsync(
            $"Infrastructure{Path.DirectorySeparatorChar}Persistence{Path.DirectorySeparatorChar}Seeding{Path.DirectorySeparatorChar}Scripts{Path.DirectorySeparatorChar}schema.sql");
        var data = await File.ReadAllTextAsync(
            $"Infrastructure{Path.DirectorySeparatorChar}Persistence{Path.DirectorySeparatorChar}Seeding{Path.DirectorySeparatorChar}Scripts{Path.DirectorySeparatorChar}data.sql");
        
        await connection.ExecuteAsync(schema);
        await connection.ExecuteAsync(data);
    }
}