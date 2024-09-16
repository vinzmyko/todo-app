using TodoApp.Web.Models;

namespace TodoApp.Web.Clients;

public class TagsClient
{
    private readonly HttpClient _httpClient;

    public TagsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TagDto[]> GetTagsAsync()
        => await _httpClient.GetFromJsonAsync<TagDto[]>("tags") ?? [];

    public async Task<TagDto> GetTagAsync(int id)
        => await _httpClient.GetFromJsonAsync<TagDto>($"tags/{id}")
            ?? throw new Exception("Could not find tag.");
    
    public async Task CreateTagAsync(CreateTagDto tag)
        => await _httpClient.PostAsJsonAsync("tags", tag);
    
    public async Task UpdateTagAsync(int id, CreateTagDto tag)
        => await _httpClient.PutAsJsonAsync($"tags/{id}", tag);
    
    public async Task DeleteTagAsync(int id)
        => await _httpClient.DeleteAsync($"tags/{id}");
}
