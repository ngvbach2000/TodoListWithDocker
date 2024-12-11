using Microsoft.EntityFrameworkCore;
using TodoListWithDocker.Entities;

namespace TodoListWithDocker.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{

    public DbSet<TodoList> TodoLists { get; set; }
}