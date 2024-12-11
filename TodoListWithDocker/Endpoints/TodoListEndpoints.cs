using TodoListWithDocker.Dtos;
using TodoListWithDocker.Extensions;
using TodoListWithDocker.Services;
using TodoListWithDocker.Services.Caching;

namespace TodoListWithDocker.Endpoints;

public static class TodoListEndpoints
{
    public static void MapTodolistEndpoints(this IEndpointRouteBuilder app)
    {
        var todoListGroup = app.MapGroup("todolist");

        todoListGroup.MapGet("", GetAllTodoLists).WithName(nameof(GetAllTodoLists));

        todoListGroup.MapGet("{id}", GetTodoListById).WithName(nameof(GetTodoListById));

        todoListGroup.MapPost("", CreateTodoList).WithName(nameof(CreateTodoList));

        todoListGroup.MapPut("{id}", UpdateTodoList).WithName(nameof(UpdateTodoList));

        todoListGroup.MapDelete("{id}", DeleteTodoListById).WithName(nameof(DeleteTodoListById));
    }
    
    public static async Task<IResult> GetAllTodoLists(
        ITodoListService todoListService,
        CancellationToken cancellationToken)
    {
        var todoLists = await todoListService.GetTodoListsAsync(cancellationToken);

        return Results.Ok(todoLists.Select(b => b.ToResponseDto()));
    }

    public static async Task<IResult> GetTodoListById(
         Guid id,
         ITodoListService todoListService,
         IRedisCacheService cacheService,
         CancellationToken cancellationToken)
    {
        var cacheKey = $"TodoList_{id}";

        var response = await cacheService.GetDataAsync<TodoListResponse>(
            cacheKey,
            cancellationToken);

        if (response is not null)
        {
            return Results.Ok(response);
        }

        var todoList = await todoListService.GetTodoListByIdAsync(id, cancellationToken);

        if (todoList is null)
        {
            return Results.NotFound();
        }

        response = todoList.ToResponseDto();

        await cacheService.SetDataAsync(
            cacheKey,
            response,
            cancellationToken);

        return Results.Ok(response);
    }

    public static async Task<IResult> CreateTodoList(
             CreateTodoListRequest request,
            ITodoListService todoListService,
            CancellationToken cancellationToken)
    {
        var todoList = request.ToEntity();

        todoList.Id = await todoListService.CreateTodoListAsync(todoList, cancellationToken);

        return Results.CreatedAtRoute(
            nameof(GetTodoListById),
            new { id = todoList.Id },
            todoList);
    }

    public static async Task<IResult> UpdateTodoList(
            Guid id,
            UpdateTodoListRequest request,
            ITodoListService todoListService,
            IRedisCacheService cacheService,
            CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"TodoList_{id}";

            var todoList = request.ToEntity(id);

            await todoListService.UpdateTodoListAsync(todoList, cancellationToken);

            await cacheService.RemoveDataAsync(cacheKey, cancellationToken);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    public static async Task<IResult> DeleteTodoListById(
            Guid id,
            ITodoListService todoListService,
            IRedisCacheService cacheService,
            CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"TodoList_{id}";

            await todoListService.DeleteTodoListByIdAsync(id, cancellationToken);

            await cacheService.RemoveDataAsync(cacheKey, cancellationToken);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}