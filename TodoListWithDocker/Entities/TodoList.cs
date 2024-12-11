namespace TodoListWithDocker.Entities;

public class TodoList
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsComplete { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    public decimal Priority { get; set; }
}