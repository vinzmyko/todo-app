using Microsoft.EntityFrameworkCore;

namespace TodoApp.Api.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app) // Makes it extend from WebApplication
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoAppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
