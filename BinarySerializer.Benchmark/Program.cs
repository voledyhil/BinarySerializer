using System.Reflection;
using BenchmarkDotNet.Running;

namespace BinarySerializer.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).RunAll();
        }
    }
}
