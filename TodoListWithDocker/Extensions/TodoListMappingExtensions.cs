using TodoListWithDocker.Dtos;
using TodoListWithDocker.Entities;

namespace TodoListWithDocker.Extensions;

public static class TodoListMappingExtensions
{
    public static TodoListResponse ToResponseDto(this TodoList todoList)
    {
        return new(
            todoList.Id,
            todoList.Name,
            todoList.Description,
            todoList.IsComplete);
    }

    public static TodoList ToEntity(this CreateTodoListRequest request)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsComplete = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static TodoList ToEntity(this UpdateTodoListRequest request, Guid id)
    {
        return new()
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            IsComplete = request.IsComplete,
            UpdatedAt = DateTime.UtcNow,
            CompletedAt = request.IsComplete ? DateTime.UtcNow : null
        };
    }
}