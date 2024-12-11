using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TodoListWithDocker.Endpoints;
using TodoListWithDocker.Extensions;
using TodoListWithDocker.Services;
using TodoListWithDocker.Services.Caching;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPostgreSqlConfig(builder.Configuration)
    .AddRedisConfig(builder.Configuration)
    .AddHealthChecks()
    .AddPostgreSqlHealth(builder.Configuration)
    .AddRedisHealth(builder.Configuration);

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<ITodoListService, TodoListService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapTodolistEndpoints();

app.Run();