namespace TodoListWithDocker.Dtos;

public sealed record CreateTodoListRequest(
    string Name, 
    string? Description);

public sealed record TodoListResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsComplete);

public sealed record UpdateTodoListRequest(
    string Name,
    string Description,
    bool IsComplete);