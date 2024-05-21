
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var todoList = new List<Todo>();

//Create
app.MapPost("/todos", (Todo task) =>
{
    todoList.Add(task);
    return TypedResults.Created("/todos/{id}", task);
});

//Retrieve one
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    var target = todoList.SingleOrDefault(t => id == t.Id);
    return target is null 
    ? TypedResults.NotFound()
    : TypedResults.Ok(target);
});

//Retrieve All
app.MapGet("/todos", () => todoList);

//Delete
app.MapDelete("/todos/{id}", (int id) =>
{
    todoList.RemoveAll(t => t.Id == id);
    return TypedResults.NoContent();
});

app.Run();

public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);
