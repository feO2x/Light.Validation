using System.Globalization;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsLessThanOrEqualToTests
{
    public static readonly TheoryData<float, float> InvalidValues =
        new ()
        {
            { 5f, 3f },
            { 19.509f, 19.508f },
            { 0f, -0.00001f }
        };

    public static readonly TheoryData<int, int> ValidValues =
        new ()
        {
            { 998, 999 },
            { -1000, -1000 },
            { 0, 1 },
            { 0, 0 }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueIsGreaterThan(float value, float comparativeValue)
    {
        var dto = new Dto<float> { Value = value };
        var context = new ValidationContext();

        context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue);

        context.ShouldHaveSingleError("value", $"value must be less than or equal to {comparativeValue.ToString(CultureInfo.InvariantCulture)}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsLessThanOrEqualTo(int value, int comparativeValue)
    {
        var dto = new Dto<int> { Value = value };
        var context = new ValidationContext();

        context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(float value, float comparativeValue)
    {
        var dto = new Dto<float> { Value = value };
        var context = new ValidationContext();

        context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue, "Errors.LessThanOrEqualTo");

        context.ShouldHaveSingleError("value", "Errors.LessThanOrEqualTo");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto<double> { Value = 24.42 };
        var context = new ValidationContext();

        context.Check(dto.Value).IsLessThanOrEqualTo(24.1, (c, o) => $"{c.Key} is too great ({o.ToString(CultureInfo.InvariantCulture)})");

        context.ShouldHaveSingleError("value", "value is too great (24.1)");
    }

    [Fact]
    public static void NoErrorWithCustomFactory()
    {
        var dto = new Dto<char> { Value = 'G' };
        var context = new ValidationContext();

        context.Check(dto.Value).IsLessThanOrEqualTo('G', (_, _) => "Foo");

        context.ShouldHaveNoError();
    }

    private sealed class Dto<T>
    {
        public T Value { get; init; } = default!;
    }
}