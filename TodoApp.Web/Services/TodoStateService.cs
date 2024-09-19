using TodoApp.Web.Models;
using Microsoft.Extensions.Logging;

namespace TodoApp.Web.Services;

public class TodoStateService
{
    private readonly ILogger<TodoStateService> _logger;

    public TodoStateService(ILogger<TodoStateService> logger)
    {
        _logger = logger;
    }

    public event Action? OnChange;
    public event Func<Task>? OnFilterChange;
    public string SelectedTimeframe { get; private set; } = "all";
    public Dictionary<string, int> TimeFrameCounts { get; private set; } = new();
    public Dictionary<string, int> TagCounts { get; private set; } = new();
    public string? SelectedTag { get; private set; }
    public List<string> ActiveTags { get; private set; } = new();
    public string? CurrentView { get; private set; }
    public bool IsDueDateAscending { get; private set; } = true;

    public void UpdateTodos(TodoSummaryDto[] todos)
    {
        UpdateTimeFrameCounts(todos);
        UpdateTagCounts(todos);
        NotifyStateChanged();
    }

    private void UpdateTimeFrameCounts(TodoSummaryDto[] todos)
    {
        TimeFrameCounts = new Dictionary<string, int>
        {
            { "all", todos.Length },
            { "today", todos.Count(t => t.TodoTimeFrame == TimeFrame.Today) },
            { "next7days", todos.Count(t => t.TodoTimeFrame == TimeFrame.NextSevenDays) },
            { "future", todos.Count(t => t.TodoTimeFrame == TimeFrame.Future) }
        };
    }

    private void UpdateTagCounts(TodoSummaryDto[] todos)
    {
        TagCounts = new Dictionary<string, int>();
        ActiveTags = new List<string>();

        foreach (var todo in todos)
        {
            if (todo.Tags != null)
            {
                foreach (var tag in todo.Tags)
                {
                    if (!TagCounts.ContainsKey(tag.Name))
                    {
                        TagCounts[tag.Name] = 1;
                        ActiveTags.Add(tag.Name);
                    }
                    else
                    {
                        TagCounts[tag.Name]++;
                    }
                }
            }
        }
    }

    public async Task SetViewAsync(string view)
    {
        CurrentView = view;
        await NotifyFilterChanged();
    }

    public async Task ToggleViewAsync(string view)
    {
        if (view == "duedate")
        {
            if (CurrentView == "duedate")
            {
                IsDueDateAscending = !IsDueDateAscending;
            }
            else
            {
                CurrentView = "duedate";
                IsDueDateAscending = true;
            }
        }
        else if (CurrentView == view)
        {
            CurrentView = null;
        }
        else
        {
            CurrentView = view;
        }
        await NotifyFilterChanged();
    }

    public async Task SetTimeframeAsync(string timeframe)
    {
        SelectedTimeframe = timeframe;
        await NotifyFilterChanged();
    }

    public async Task ToggleTagAsync(string tag)
    {
        _logger.LogInformation("ToggleTagAsync called with tag: {Tag}", tag);
        _logger.LogInformation("Current SelectedTag: {SelectedTag}", SelectedTag);

        if (SelectedTag == tag)
        {
            // Clicking the same tag again clears the filter
            SelectedTag = null;
            _logger.LogInformation("Same tag clicked, cleared selection");
        }
        else
        {
            // Clicking a different tag sets it as the new filter
            SelectedTag = tag;
            _logger.LogInformation("Different tag clicked, new selection: {SelectedTag}", SelectedTag);
        }

        await NotifyFilterChanged();
    }

    private void NotifyStateChanged()
    {
        _logger.LogInformation("NotifyStateChanged called");
        OnChange?.Invoke();
    }

    private async Task NotifyFilterChanged()
    {
        _logger.LogInformation("NotifyFilterChanged called");
        NotifyStateChanged();
        if (OnFilterChange != null)
        {
            await OnFilterChange.Invoke();
        }
    }
}