using TodoApp.Web.Components;
using TodoApp.Web.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Set up logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Create a logger
var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
});
var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting application");
logger.LogInformation($"Environment: {builder.Environment.EnvironmentName}");

// Add user secrets in development
if (builder.Environment.IsDevelopment())
{
    logger.LogInformation("Adding user secrets to configuration");
    builder.Configuration.AddUserSecrets<Program>();
}

var todoAppApiUrl = builder.Configuration["ApiSettings:BaseUrl"];
logger.LogInformation($"API URL from configuration: {todoAppApiUrl}");

if (string.IsNullOrEmpty(todoAppApiUrl))
{
    logger.LogError("BaseUrl is not set in ApiSettings");
    throw new Exception("BaseUrl is not set in ApiSettings");
}

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<UsersClient>(client => 
{
    logger.LogInformation($"Configuring UsersHttpClient with BaseAddress: {todoAppApiUrl}");
    client.BaseAddress = new Uri(todoAppApiUrl);
});

builder.Services.AddHttpClient<TagsClient>(client => 
{
    logger.LogInformation($"Configuring TagsHttpClient with BaseAddress: {todoAppApiUrl}");
    client.BaseAddress = new Uri(todoAppApiUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
    logger.LogInformation("HTTPS Redirection is disabled in Development environment");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

logger.LogInformation("Application configured and ready to run");

app.Run();