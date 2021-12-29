using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Light.GuardClauses;
using Light.Validation.Checks;
using Light.Validation.Tools;
using Microsoft.AspNetCore.Mvc;
using Synnotech.DatabaseAbstractions;
using Synnotech.DatabaseAbstractions.Mocks;
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
        var context = new ValidationContext();
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = "" };

        var result = invalidDto.CheckForErrors(context, out var errors);

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

    [Fact]
    public async Task ValidateAsync()
    {
        var session = new UpdateUserNameSessionMock() { DoesUserNameExist = true };
        var sessionFactory = new SessionFactoryMock<IUpdateUserNameSession>(session);
        var controller = new UpdateUserNameController(sessionFactory);
        var dto = new UpdateUserNameDto { Id = 42, UserName = "Kevin" };

        var result = await controller.UpdateUserName(dto);

        var badRequestResult = result.MustBeOfType<BadRequestObjectResult>();
        Output.WriteLine(Json.Serialize(badRequestResult.Value));
    }

    private sealed class UpdateUserNameController : ControllerBase
    {
        public UpdateUserNameController(ISessionFactory<IUpdateUserNameSession> sessionFactory) =>
            SessionFactory = sessionFactory;

        private ISessionFactory<IUpdateUserNameSession> SessionFactory { get; }

        public async Task<IActionResult> UpdateUserName(UpdateUserNameDto dto)
        {
            var context = new ValidationContext();
            if (dto.CheckForErrors(context, out var errors))
                return BadRequest(errors);

            await using var session = await SessionFactory.OpenSessionAsync();
            if (await session.CheckIfUserNameExistsAsync(dto.UserName))
            {
                context.Check(dto.UserName).AddError("The user name already exists");
                return BadRequest(context.Errors);
            }

            return NoContent();
        }
    }

    private static void CheckKeys(Dictionary<string, object> errors, params string[] expectedKeys) =>
        errors.Keys.OrderBy(key => key).Should().Equal(expectedKeys.OrderBy(key => key));

    private sealed class DtoValidator : Validator<UpdateUserNameDto>
    {
        protected override void PerformValidation(ValidationContext context, UpdateUserNameDto dto)
        {
            context.Check(dto.Id).GreaterThan(0);
            dto.UserName = context.Check(dto.UserName).TrimAndCheckNotWhiteSpace();
        }
    }

    private sealed class UpdateUserNameDto
    {
        public int Id { get; init; }
        public string UserName { get; set; } = string.Empty;

        public bool CheckForErrors(ValidationContext context, out Dictionary<string, object>? errors)
        {
            context.Check(Id).GreaterThan(0);
            UserName = context.Check(UserName).TrimAndCheckNotWhiteSpace();
            return context.TryGetErrors(out errors);
        }
    }

    private interface IUpdateUserNameSession : IAsyncSession
    {
        Task<bool> CheckIfUserNameExistsAsync(string userName);
    }

    private sealed class UpdateUserNameSessionMock : AsyncSessionMock, IUpdateUserNameSession
    {
        public bool DoesUserNameExist { get; set; }

        public Task<bool> CheckIfUserNameExistsAsync(string userName) =>
            Task.FromResult(DoesUserNameExist);
    }
}