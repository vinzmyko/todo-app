using TodoApp.Web.Clients;
using TodoApp.Web.Models;

namespace TodoApp.Web.Services;

public class UserService
{
    private readonly UsersClient _usersClient;
    private UserDto? _currentUser;

    public event Action? OnUserStateChanged;

    public UserService(UsersClient usersClient)
    {
        _usersClient = usersClient;
    }

    public UserDto? CurrentUser => _currentUser;

    public bool IsLoggedIn => _currentUser != null;

    public async Task LoginAsync(string username)
    {
        var users = await _usersClient.GetUsersAsync();
        _currentUser = users.FirstOrDefault(u => u.Username == username);
        
        if (_currentUser == null)
        {
            var newUser = new CreateUserDto { Username = username };
            await _usersClient.CreateUserAsync(newUser);
            _currentUser = (await _usersClient.GetUsersAsync()).First(u => u.Username == username);
        }

        OnUserStateChanged?.Invoke();
    }

    public void Logout()
    {
        _currentUser = null;
        OnUserStateChanged?.Invoke();
    }
}