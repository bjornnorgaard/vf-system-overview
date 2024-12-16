namespace Todos.Library;

public class TodoLibrary : ITodoLibrary
{
    private readonly List<Todo> _todos = [];

    public void Add(Todo? todo)
    {
        if (todo == null) return;
        _todos.Add(todo);
    }

    public IEnumerable<Todo> GetAll()
    {
        return _todos;
    }

    public Todo? GetById(Guid id)
    {
        return _todos.FirstOrDefault(t => t.Id == id);
    }

    public void Remove(Guid id)
    {
        _todos.RemoveAll(t => t.Id == id);
    }

    public void Update(Todo? todo)
    {
        if (todo == null) return;
        var index = _todos.FindIndex(t => t.Id == todo.Id);
        if (index == -1) return;
        _todos[index] = todo;
    }
}
