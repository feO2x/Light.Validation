using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.Validation.Tests;

public sealed class SimpleValidationTests
{
    public SimpleValidationTests(ITestOutputHelper output) => Output = output;

    private ITestOutputHelper Output { get; }

    [Fact]
    public void ValidateInvalidLoginDto()
    {
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = "" };

        var result = invalidDto.CheckForErrors(out var errors);

        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
        errors.Should().HaveCount(2);
    }

    private sealed class UpdateUserNameDto
    {
        public int Id { get; init; }
        public string UserName { get; set; } = string.Empty;

        public bool CheckForErrors(out Dictionary<string, object>? errors)
        {
            var context = new ValidationContext();
            context.Check(Id).GreaterThan(0);
            UserName = context.Check(UserName).TrimAndCheckNotWhiteSpace();
            return context.TryGetErrors(out errors);
        }
    }
}