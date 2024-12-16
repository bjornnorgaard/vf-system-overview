using BenchmarkDotNet.Running;

namespace Todos.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}
