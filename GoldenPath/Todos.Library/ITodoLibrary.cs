namespace Todos.Library;

public interface ITodoLibrary
{
    void Add(Todo? todo);
    IEnumerable<Todo> GetAll();
    Todo? GetById(Guid id);
    void Remove(Guid id);
    void Update(Todo? todo);
}
