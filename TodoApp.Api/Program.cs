using TodoApp.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TodoApp");
builder.Services.AddSqlite<TodoAppDbContext>(connectionString);

var app = builder.Build();

app.MigrateDb();

app.Run();
