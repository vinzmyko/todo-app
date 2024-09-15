using System.Net.Http.Json;
using TodoApp.Web.Models;

namespace TodoApp.Web.Clients;

public class UsersClient
{
    private readonly HttpClient _httpClient;

    public UsersClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto[]> GetUsersAsync()
        => await _httpClient.GetFromJsonAsync<UserDto[]>("users") ?? [];

    public async Task<UserDto> GetUserAsync(int id)
        => await _httpClient.GetFromJsonAsync<UserDto>($"users/{id}")
            ?? throw new Exception("Could not find user!");

    public async Task CreateUserAsync(CreateUserDto user)
        => await _httpClient.PostAsJsonAsync("users", user);

    public async Task UpdateUserAsync(int id, CreateUserDto user)
        => await _httpClient.PutAsJsonAsync($"users/{id}", user);

    public async Task DeleteUserAsync(int id)
        => await _httpClient.DeleteAsync($"users/{id}");
}