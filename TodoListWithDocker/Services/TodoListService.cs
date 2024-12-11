using Microsoft.EntityFrameworkCore;
using TodoListWithDocker.Database;
using TodoListWithDocker.Entities;

namespace TodoListWithDocker.Services;

public class TodoListService(ApplicationDbContext context) : ITodoListService
{
    public async Task<TodoList?> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.TodoLists.AsNoTracking().FirstOrDefaultAsync(
            b => b.Id == id, 
            cancellationToken);

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync(CancellationToken cancellationToken)
        => await context.TodoLists
            .AsNoTracking()
            .OrderBy(o=>o.Id)
            .ToListAsync(cancellationToken);

    public async Task<Guid> CreateTodoListAsync(TodoList todoList, CancellationToken cancellationToken)
    {
        context.Add(todoList);

        await context.SaveChangesAsync(cancellationToken);

        return todoList.Id;
    }

    public async Task UpdateTodoListAsync(TodoList todoList, CancellationToken cancellationToken)
    {
        var todoListObj = await context.TodoLists
            .FirstOrDefaultAsync(b => b.Id == todoList.Id, cancellationToken);

        if (todoListObj is null)
        {
            throw new ArgumentException($"TodoList is not found Id {todoList.Id}");
        }

        todoListObj.Name = todoList.Name;
        todoListObj.Description = todoList.Description;
        todoListObj.IsComplete = todoList.IsComplete;
        todoListObj.UpdatedAt = todoList.UpdatedAt;
        todoListObj.CompletedAt = todoList.CompletedAt;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTodoListByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var todoList = await context.TodoLists
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (todoList is null)
        {
            throw new ArgumentException($"TodoList is not found Id {id}");
        }

        context.Remove(todoList);

        await context.SaveChangesAsync(cancellationToken);
    }
}