using TodoApp.Api.Data;
using TodoApp.Api.Dtos.Todo;
using TodoApp.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Api.Endpoints;

public static class TodoEndpoints
{
    const string GetTodoEndpointName = "GetTodo";

    public static RouteGroupBuilder MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("todos")
                       .WithParameterValidation();

        // GET /todos (summary list)
        group.MapGet("/", async (TodoAppDbContext dbContext) =>
            await dbContext.Todos
                           .AsNoTracking()
                           .Select(todo => todo.ToTodoSummaryDto())
                           .ToListAsync());

        // GET /todos/{id} (detailed view)
        group.MapGet("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            var todo = await dbContext.Todos
                                      .Include(t => t.User)
                                      .Include(t => t.TodoTags)
                                      .ThenInclude(tt => tt.Tag)
                                      .FirstOrDefaultAsync(t => t.Id == id);

            return todo is null ? Results.NotFound() : Results.Ok(todo.ToTodoDto());
        })
        .WithName(GetTodoEndpointName);

        // POST /todos
        group.MapPost("/", async (CreateTodoDto newTodo, TodoAppDbContext dbContext) =>
        {
            var user = await dbContext.Users.FindAsync(newTodo.UserId);
            if (user is null)
            {
                return Results.NotFound($"User with ID {newTodo.UserId} not found.");
            }

            var tags = await dbContext.Tags
                                      .Where(t => newTodo.TagIds.Contains(t.Id))
                                      .ToListAsync();

            var todo = newTodo.ToEntity(user, tags);

            dbContext.Todos.Add(todo);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetTodoEndpointName,
                new { id = todo.Id },
                todo.ToTodoDto());
        });

        // PUT /todos/{id}
        group.MapPut("/{id}", async (int id, UpdateTodoDto updatedTodo, TodoAppDbContext dbContext) =>
        {
            var existingTodo = await dbContext.Todos
                                              .Include(t => t.TodoTags)
                                              .FirstOrDefaultAsync(t => t.Id == id);
            
            if (existingTodo is null)
            {
                return Results.NotFound();
            }

            var tags = await dbContext.Tags
                                      .Where(t => updatedTodo.TagIds.Contains(t.Id))
                                      .ToListAsync();

            existingTodo.UpdateEntityFromDto(updatedTodo, tags);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /todos/{id}
        group.MapDelete("/{id}", async (int id, TodoAppDbContext dbContext) =>
        {
            var todo = await dbContext.Todos.FindAsync(id);
            if (todo is null)
            {
                return Results.NotFound();
            }

            dbContext.Todos.Remove(todo);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // GET /todos/user/{userId} (get todos for a specific user)
        group.MapGet("/user/{userId}", async (int userId, TodoAppDbContext dbContext) =>
            await dbContext.Todos
                           .Where(t => t.UserId == userId)
                           .AsNoTracking()
                           .Select(todo => todo.ToTodoSummaryDto())
                           .ToListAsync());

        // RESET /todos/reset
        #if DEBUG
        group.MapPost("/reset", async (TodoAppDbContext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Todos; DELETE FROM TodoTags; DELETE FROM sqlite_sequence WHERE name='Todos';");
            await dbContext.SaveChangesAsync();
            return Results.Ok("Todos reset completed.");
        });
        #endif

        return group;
    }
}