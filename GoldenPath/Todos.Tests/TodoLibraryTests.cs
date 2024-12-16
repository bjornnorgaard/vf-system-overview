using FluentAssertions;
using Todos.Library;
using Xunit;

namespace Todos.Tests;

public class TodoLibraryTests
{
    private readonly ITodoLibrary _uut = new TodoLibrary();

    [Fact]
    public void GetTodoFromEmptyList_ShouldReturnNull()
    {
        var todo = _uut.GetById(Guid.NewGuid());
        todo.Should().BeNull();
    }

    [Fact]
    public void AddOneTodoAndGetById_ShouldReturnTodo()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        var result = _uut.GetById(todo.Id);
        result.Should().Be(todo);
    }

    [Fact]
    public void AddNullAndGetById_ShouldReturnNull()
    {
        Todo? todo = null;
        _uut.Add(todo);
    }

    [Fact]
    public void AddOneTodoAndGetAll_ShouldReturnOneTodo()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        var result = _uut.GetAll();
        result.Should().Contain(todo);
    }

    [Fact]
    public void AddOneTodoAndRemove_ShouldReturnEmptyList()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        _uut.Remove(todo.Id);
        var result = _uut.GetAll();
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddTwoTodosAndRemoveOne_ShouldReturnOneTodo()
    {
        var todo1 = TodoConstructor.CreateValidTodo();
        var todo2 = TodoConstructor.CreateValidTodo();
        _uut.Add(todo1);
        _uut.Add(todo2);
        _uut.Remove(todo1.Id);
        var result = _uut.GetAll();
        result.Should().Contain(todo2);
    }

    [Fact]
    public void AddOneTodoAndUpdate_ShouldReturnUpdatedTodo()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        todo.Title = "Updated title";
        _uut.Update(todo);
        var result = _uut.GetById(todo.Id);
        result.Should().Be(todo);
    }

    [Fact]
    public void AddOneTodoAndUpdateWithDifferentId_ShouldReturnNoChange()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        var updatedTodo = TodoConstructor.CreateValidTodo();
        updatedTodo.Id = Guid.NewGuid();
        _uut.Update(updatedTodo);
        var result = _uut.GetById(todo.Id);
        result.Should().Be(todo);
    }

    [Fact]
    public void AddOneTodoAndUpdateWithNull_ShouldReturnNoChange()
    {
        var todo = TodoConstructor.CreateValidTodo();
        _uut.Add(todo);
        _uut.Update(null);
        var result = _uut.GetById(todo.Id);
        result.Should().Be(todo);
    }
}
