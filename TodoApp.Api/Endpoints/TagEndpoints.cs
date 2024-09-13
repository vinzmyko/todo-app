using TodoApp.Api.Data;
using TodoApp.Api.Entities;
using TodoApp.Api.Mapping;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Dtos.Tag;

namespace TodoApp.Api.Endpoints;

public static class TagEndpoints
{
    const string GetUserEndpointName = "GetTag";

    public static RouteGroupBuilder MapTagEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("tags")
                       .WithParameterValidation();

        // GET /tags
        // Being async means operations are non-blocking, improving app responsiveness
        group.MapGet("/", async (TodoAppDbContext dbContext) =>
            await dbContext.Tags
                           .Select(tag => tag.ToTagDto())
                           .AsNoTracking() // Tells EFC not to track these entities in memory, which improves performance for read-only operations.
                           .ToListAsync());

        // GET /tags/{id}
        group.MapGet("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            Tag? tag = await dbContext.Tags.FindAsync(id);

            return tag is null ? Results.NotFound() : Results.Ok(tag.ToTagDto());
        })
        .WithName(GetUserEndpointName);

        // POST /tags
        group.MapPost("/", async (CreateTagDto newTag, TodoAppDbContext dbContext) =>
        {
            // Check if tag already exists
            if (await dbContext.Tags.AnyAsync(t => t.Name == newTag.Name))
            {
                return Results.Conflict($"Tag '{newTag.Name}' already exists.");
            }

            Tag tag = newTag.ToEntity();

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetUserEndpointName,
                new { id = tag.Id },
                tag.ToTagDto());
        });

       // PUT /tags/{id}
        group.MapPut("/{id}", async (int id, CreateTagDto updatedTag, TodoAppDbContext dbContext) =>
        {
            var existingTag = await dbContext.Tags.FindAsync(id);
            
            if (existingTag is null)
            {
                return Results.NotFound();
            }

            // Check if tag is being changed
            if (existingTag.Name != updatedTag.Name)
            {
                if (await dbContext.Tags.AnyAsync(t => t.Name == updatedTag.Name))
                {
                    return Results.Conflict($"Tag '{updatedTag.Name}' already exists.");
                }
            }

            existingTag.UpdateEntityFromDto(updatedTag);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /tags/{id}
        group.MapDelete("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            await dbContext.Tags
                           .Where(tag => tag.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        // RESET /tags/reset
        #if DEBUG
        group.MapPost("/reset", async (TodoAppDbContext dbContext) =>
        {
            // Delete all tags
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Tags; DELETE FROM sqlite_sequence WHERE name='Tags';");

            // Re-seed the initial tags
            var seededTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "work" },
                new Tag { Id = 2, Name = "personal" },
                new Tag { Id = 3, Name = "urgent" },
                new Tag { Id = 4, Name = "shopping" },
                new Tag { Id = 5, Name = "fitness" }
            };

            dbContext.Tags.AddRange(seededTags);
            await dbContext.SaveChangesAsync();

            return Results.Ok("Tags reset and re-seeded.");
        });
        #endif

        return group;
    }
}