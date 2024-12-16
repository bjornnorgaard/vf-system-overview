using Todos.Library;

namespace Todos.Tests;

public static class TodoConstructor
{
    public static Todo CreateValidTodo()
    {
        var id = Guid.NewGuid();
        var todo = new Todo
        {
            Id = id,
            Title = $"Todo title {id}",
            Description = $"Test description {id}",
            IsComplete = false
        };
        return todo;
    }
}