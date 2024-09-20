using TodoApp.Web.Models;
using TodoApp.Web.Services;

namespace TodoApp.Web.Clients;

public class TodosClient
{
    private readonly HttpClient _httpClient;
    private readonly UserService _userService;

    public TodosClient(HttpClient httpClient, UserService userService)
    {
        _httpClient = httpClient;
        _userService = userService;
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
        todo.UserId = _userService.CurrentUser!.Id;
        todo.UserTimeZone = TimeZoneInfo.Local.Id;
        todo.UserTimeOffsetMinutes = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
        todo.UserLocalTime = DateTime.Now;

        var response = await _httpClient.PostAsJsonAsync("todos", todo);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoDto>()
            ?? throw new Exception("Failed to create todo.");
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