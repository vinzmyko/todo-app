using TodoApp.Web.Clients;
using TodoApp.Web.Models;

namespace TodoApp.Web.Services;

public class UserService
{
    private readonly UsersClient _usersClient;
    private UserDto? _currentUser;
    public event Action? OnUserStateChanged;
    // '=>' property expression evaluated each time the property is accessed
    public UserDto? CurrentUser => _currentUser;
    public bool IsLoggedIn => _currentUser != null;
    // constructor injection is typically preferred in service classes for better testability and explicit dependency declaration
    public UserService(UsersClient usersClient)
    {
        _usersClient = usersClient;
    }

    public async Task LoginAsync(string username)
    {
        var users = await _usersClient.GetUsersAsync();
        _currentUser = users.FirstOrDefault(u => u.Username == username);

        // When a _currentUser is found or created 'UserDto CurrentUser' and `bool IsLoggedIn` is assigned it's values
        
        if (_currentUser == null)
        {
            var newUser = new CreateUserDto { Username = username };
            await _usersClient.CreateUserAsync(newUser);
            _currentUser = (await _usersClient.GetUsersAsync()).First(u => u.Username == username);
        }

        // Emits UserStateChanged because the property expression evaluations
        OnUserStateChanged?.Invoke();
    }

    public void Logout()
    {
        _currentUser = null;
        OnUserStateChanged?.Invoke();
    }
}