using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Light.Validation.Benchmarks;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(typeof(Program).Assembly, CreateDefaultConfig(), args);

    public static IConfig CreateDefaultConfig() =>
        DefaultConfig.Instance
                     .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60))
                     .AddDiagnoser(MemoryDiagnoser.Default);
}