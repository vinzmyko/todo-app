﻿using TodoApp.Api.Entities;
using TodoApp.Api.Dtos.Todo;
using TodoApp.Api.Dtos.Tag;

namespace TodoApp.Api.Mapping;

public static class TodoMapping
{
    public static Todo ToEntity(this CreateTodoDto todoDto, User user, IEnumerable<Tag> tags)
    {
        var todo = new Todo
        {
            Title = todoDto.Title,
            Description = todoDto.Description,
            DueDate = todoDto.DueDate,
            TodoTimeFrame = todoDto.TodoTimeFrame,
            User = user,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var tag in tags)
        {
            todo.TodoTags.Add(new TodoTag { Todo = todo, Tag = tag });
        }

        return todo;
    }

    public static TodoDto ToTodoDto(this Todo todo)
    {
        return new TodoDto(
            todo.Id,
            todo.Title,
            todo.Description,
            todo.IsCompleted,
            todo.CreatedAt,
            todo.CompletedAt,
            todo.DueDate,
            todo.TodoTimeFrame,
            todo.UserId,
            todo.User.Username,
            todo.TodoTags.Select(tt => new TagDto(tt.Tag.Id, tt.Tag.Name))
        );
    }

    public static void UpdateEntityFromDto(this Todo todo, UpdateTodoDto updateDto, IEnumerable<Tag> tags)
    {
        todo.Title = updateDto.Title;
        todo.Description = updateDto.Description;
        todo.DueDate = updateDto.DueDate;
        todo.TodoTimeFrame = updateDto.TodoTimeFrame;

        // Clear existing tags and add new ones
        todo.TodoTags.Clear();
        foreach (var tag in tags)
        {
            todo.TodoTags.Add(new TodoTag { Todo = todo, Tag = tag });
        }
    }

    public static TodoSummaryDto ToTodoSummaryDto(this Todo todo)
    {
        return new TodoSummaryDto(
            todo.Id,
            todo.Title,
            todo.IsCompleted,
            todo.DueDate,
            todo.TodoTimeFrame
        );
    }
}