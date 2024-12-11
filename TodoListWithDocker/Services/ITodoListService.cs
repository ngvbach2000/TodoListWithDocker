using TodoListWithDocker.Entities;

namespace TodoListWithDocker.Services;

public interface ITodoListService
{
    public Task<TodoList?> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<IEnumerable<TodoList>> GetTodoListsAsync(CancellationToken cancellationToken);
    public Task<Guid> CreateTodoListAsync(TodoList todoList, CancellationToken cancellationToken);
    public Task UpdateTodoListAsync(TodoList todoList, CancellationToken cancellationToken);
    public Task DeleteTodoListByIdAsync(Guid id, CancellationToken cancellationToken);
}