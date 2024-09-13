using TodoApp.Api.Data;
using TodoApp.Api.Dtos.User;
using TodoApp.Api.Entities;
using TodoApp.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Api.Endpoints;

public static class UserEndpoints
{
    const string GetUserEndpointName = "GetUser";

    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users")
                       .WithParameterValidation();

        // GET /users
        group.MapGet("/", async (TodoAppDbContext dbContext) =>
            await dbContext.Users
                           .Select(user => user.ToUserDto())
                           .AsNoTracking()
                           .ToListAsync());

        // GET /users/{id}
        group.MapGet("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            User? user = await dbContext.Users.FindAsync(id);

            return user is null ? Results.NotFound() : Results.Ok(user.ToUserDto());
        })
        .WithName(GetUserEndpointName);

        // POST /users
        group.MapPost("/", async (CreateUserDto newUser, TodoAppDbContext dbContext) =>
        {
            // Check if username already exists
            if (await dbContext.Users.AnyAsync(u => u.Username == newUser.Username))
            {
                return Results.Conflict($"Username '{newUser.Username}' is already taken.");
            }

            User user = newUser.ToEntity();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetUserEndpointName,
                new { id = user.Id },
                user.ToUserDto());
        });

       // PUT /users/{id}
        group.MapPut("/{id}", async (int id, CreateUserDto updatedUser, TodoAppDbContext dbContext) =>
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            
            if (existingUser is null)
            {
                return Results.NotFound();
            }

            // Check if username is being changed
            if (existingUser.Username != updatedUser.Username)
            {
                // Check if the new username already exists
                if (await dbContext.Users.AnyAsync(u => u.Username == updatedUser.Username))
                {
                    return Results.Conflict($"Username '{updatedUser.Username}' is already taken.");
                }
            }

            existingUser.UpdateEntityFromDto(updatedUser);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /users/{id}
        group.MapDelete("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            await dbContext.Users
                           .Where(user => user.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        // RESET /users/reset
        #if DEBUG
        group.MapPost("/reset", async (TodoAppDbContext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Users; DELETE FROM sqlite_sequence WHERE name='Users';");
            return Results.Ok("Database reset completed.");
        });
        #endif

        return group;
    }
}