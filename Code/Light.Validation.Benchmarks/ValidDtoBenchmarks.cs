using BenchmarkDotNet.Attributes;

namespace Light.Validation.Benchmarks;

public class ValidDtoBenchmarks
{
    public UpdateUserNameDto Dto { get; } = new () { Id = 42, UserName = "feO2x" };
    public LightValidator LightValidator { get; } = new (ValidationContextFactory.Instance);
    public FluentValidator FluentValidator { get; } = new ();

    [Benchmark(Baseline = true)]
    public object? CheckLightDtoDirectly()
    {
        Dto.CheckForErrors(out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(Dto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(Dto);
}