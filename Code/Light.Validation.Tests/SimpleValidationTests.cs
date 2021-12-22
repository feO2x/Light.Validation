using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.Validation.Tests;

public sealed class SimpleValidationTests
{
    public SimpleValidationTests(ITestOutputHelper output) => Output = output;

    private ITestOutputHelper Output { get; }

    [Fact]
    public void ValidateDtoDirectly()
    {
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = "" };

        var result = invalidDto.CheckForErrors(out var errors);

        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
        errors.Should().HaveCount(2);
        CheckKeys(errors!, "id", "userName");
    }

    [Fact]
    public void ValidateViaValidator()
    {
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = " " };
        var validator = new DtoValidator();

        var result = validator.CheckForErrors(invalidDto, out var errors);

        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
        errors.Should().HaveCount(2);
        CheckKeys(errors!, "id", "userName");
    }

    private static void CheckKeys(Dictionary<string, object> errors, params string[] expectedKeys) =>
        errors.Keys.OrderBy(key => key).Should().Equal(expectedKeys.OrderBy(key => key));

    private sealed class DtoValidator : Validator<UpdateUserNameDto>
    {
        protected override void CheckForErrors(ValidationContext context, UpdateUserNameDto dto)
        {
            context.Check(dto.Id).GreaterThan(0);
            dto.UserName = context.Check(dto.UserName).TrimAndCheckNotWhiteSpace();
        }
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