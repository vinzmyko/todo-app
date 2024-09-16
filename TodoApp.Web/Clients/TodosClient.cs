using TodoApp.Web.Models;

namespace TodoApp.Web.Clients;

public class TodosClient
{
    private readonly HttpClient _httpClient;
    private int _currentUserId;

    public TodosClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetCurrentUser(int userId)
    {
        _currentUserId = userId;
    }

    // GET all todos for the current user (summary)
    public async Task<TodoSummaryDto[]> GetAllTodosAsync()
        => await _httpClient.GetFromJsonAsync<TodoSummaryDto[]>($"todos/user/{_currentUserId}") ?? [];

    // GET a specific todo (detailed)
    public async Task<TodoDto> GetTodoAsync(int id)
        => await _httpClient.GetFromJsonAsync<TodoDto>($"todos/{id}")
            ?? throw new Exception("Could not find todo!");

    // POST a new todo for the current user
    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todo)
    {
        todo.UserId = _currentUserId;
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
