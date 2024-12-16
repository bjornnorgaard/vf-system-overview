using BenchmarkDotNet.Attributes;
using Todos.Library;

namespace Todos.Benchmark;

[MemoryDiagnoser, MinColumn, MaxColumn]
public class GetterBenchmarks
{
    [Params(100, 1_000)] public int TodoCount { get; set; }

    public ITodoLibrary Lib { get; set; } = new TodoLibrary();

    public Todo Needle { get; set; } = new Todo
    {
        Id = Guid.NewGuid(),
        Title = "Needle",
        Description = "What we're looking for",
        IsComplete = false,
    };

    public GetterBenchmarks()
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = $"Title {Guid.NewGuid()}",
            Description = $"Description {Guid.NewGuid()}",
            IsComplete = false
        };

        for (var i = 0; i < TodoCount; i++)
        {
            if (i == TodoCount / 2) // Adding needle halfway through.
            {
                Lib.Add(Needle);
            }

            Lib.Add(todo);
        }
    }

    [Benchmark]
    public void GetById()
    {
        var todo = Lib.GetById(Needle.Id);
    }

    [Benchmark]
    public void GetAll()
    {
        var todos = Lib.GetAll();
        var todo = todos.FirstOrDefault(t => t.Id == Needle.Id);
    }
}
