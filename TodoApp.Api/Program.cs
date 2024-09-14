using TodoApp.Api.Data;
using TodoApp.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TodoApp");
builder.Services.AddSqlite<TodoAppDbContext>(connectionString);

var app = builder.Build();

app.MapUserEndpoints();
app.MapTagEndpoints();
app.MapTodoEndpoints();

await app.MigrateDbAsync();

app.Run();
