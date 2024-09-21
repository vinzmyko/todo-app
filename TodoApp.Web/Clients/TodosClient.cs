using System.Text.Json;
using TodoApp.Web.Models;
using TodoApp.Web.Services;

namespace TodoApp.Web.Clients;

public class TodosClient
{
    private readonly HttpClient _httpClient;
    private readonly UserService _userService;
    private readonly ILogger<TodosClient> _logger;

    public TodosClient(HttpClient httpClient, UserService userService, ILogger<TodosClient> logger)
    {
        _httpClient = httpClient;
        _userService = userService;
        _logger = logger;
    }

    // GET all todos for the current user (summary)
    public async Task<TodoSummaryDto[]> GetAllTodosAsync()
    {
        if (!_userService.IsLoggedIn)
        {
            return Array.Empty<TodoSummaryDto>();
        }
        return await _httpClient.GetFromJsonAsync<TodoSummaryDto[]>($"todos/user/{_userService.CurrentUser!.Id}") ?? [];
    }


    // GET a specific todo (detailed)
    public async Task<TodoDto> GetTodoAsync(int id)
        => await _httpClient.GetFromJsonAsync<TodoDto>($"todos/{id}")
            ?? throw new Exception("Could not find todo!");

    // POST a new todo for the current user
    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todo)
    {
        try
        {
            // Ensure the UserId is set
            todo.UserId = _userService.CurrentUser!.Id;

            // Ensure UserLocalTime is set with the correct Kind
            todo.UserLocalTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            // Set time zone information
            todo.UserTimeZone = TimeZoneInfo.Local.Id;
            todo.UserTimeOffsetMinutes = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;

            // If DueDate is set, ensure it's also Unspecified
            if (todo.DueDate.HasValue)
            {
                todo.DueDate = DateTime.SpecifyKind(todo.DueDate.Value, DateTimeKind.Unspecified);
            }

            _logger.LogInformation("Sending CreateTodo request: {@Todo}", todo);

            var response = await _httpClient.PostAsJsonAsync("todos", todo);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error creating todo. Status: {StatusCode}, Content: {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Error creating todo. Status: {response.StatusCode}, Content: {errorContent}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdTodo = await response.Content.ReadFromJsonAsync<TodoDto>(options);
            if (createdTodo == null)
            {
                throw new Exception("Failed to deserialize the created todo.");
            }

            _logger.LogInformation("Todo created successfully: {@CreatedTodo}", createdTodo);
            return createdTodo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateTodoAsync");
            throw;
        }
    }

    // PUT (update) an existing todo
    public async Task UpdateTodoAsync(int id, UpdateTodoDto todo)
    {
        var response = await _httpClient.PutAsJsonAsync($"todos/{id}", todo);
        response.EnsureSuccessStatusCode();
    }

    // DELETE a todo
    public async Task DeleteTodoAsync(int id)
        => await _httpClient.DeleteAsync($"todos/{id}");
}