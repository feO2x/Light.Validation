using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Light.GuardClauses;
using Light.Validation.Checks;
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
        var context = ValidationContextFactory.CreateContext();
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = "" };

        var result = invalidDto.CheckForErrors(context, out var errors);

        var errorsDictionary = errors.MustBeOfType<Dictionary<string, object>>();
        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
        errorsDictionary.Should().HaveCount(2);
        CheckKeys(errorsDictionary, nameof(UpdateUserNameDto.Id), nameof(UpdateUserNameDto.UserName));
    }

    [Fact]
    public void ValidateViaValidator()
    {
        var invalidDto = new UpdateUserNameDto { Id = 0, UserName = " " };
        var validator = new DtoValidator(ValidationContextFactory.Instance);

        var result = validator.CheckForErrors(invalidDto, out var errors);

        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
        var errorDictionary = errors.MustBeOfType<Dictionary<string, object>>();
        errorDictionary.Should().HaveCount(2);
        CheckKeys(errorDictionary, nameof(UpdateUserNameDto.Id), nameof(UpdateUserNameDto.UserName));
    }

    [Fact]
    public async Task ValidateAsync()
    {
        var session = new UpdateUserNameSessionMock { DoesUserNameExist = true };
        var sessionFactory = new SessionFactoryMock<IUpdateUserNameSession>(session);
        var validator = new DtoValidator(ValidationContextFactory.Instance);
        var controller = new UpdateUserNameController(sessionFactory, validator);
        var dto = new UpdateUserNameDto { Id = 42, UserName = "Kevin" };

        var result = await controller.UpdateUserName(dto);

        var badRequestResult = result.MustBeOfType<BadRequestObjectResult>();
        Output.WriteLine(Json.Serialize(badRequestResult.Value));
    }

    [Fact]
    public static void ValidateNull()
    {
        UpdateUserNameDto dto = null!;
        var validator = new DtoValidator(ValidationContextFactory.Instance);

        var result = validator.CheckForErrors(dto, out var errors);

        result.Should().BeTrue();
        errors.MustBeOfType<string>().Should().Be("dto must not be null");
    }

    private sealed class UpdateUserNameController : ControllerBase
    {
        public UpdateUserNameController(ISessionFactory<IUpdateUserNameSession> sessionFactory,
                                        DtoValidator validator)
        {
            SessionFactory = sessionFactory;
            Validator = validator;
        }

        private ISessionFactory<IUpdateUserNameSession> SessionFactory { get; }
        private DtoValidator Validator { get; }

        public async Task<IActionResult> UpdateUserName(UpdateUserNameDto? dto)
        {
            var context = ValidationContextFactory.CreateContext();
            if (Validator.CheckForErrors(dto, context, out var errors))
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
        public DtoValidator(IValidationContextFactory validationContextFactory) : base(validationContextFactory) { }

        protected override UpdateUserNameDto PerformValidation(ValidationContext context, UpdateUserNameDto dto)
        {
            context.Check(dto.Id).IsGreaterThan(0);
            dto.UserName = context.Check(dto.UserName).IsNotNullOrWhiteSpace();
            return dto;
        }
    }

    private sealed class UpdateUserNameDto
    {
        public int Id { get; init; }
        public string UserName { get; set; } = string.Empty;

        public bool CheckForErrors(ValidationContext context, out object? errors)
        {
            context.Check(Id).IsGreaterThan(0);
            UserName = context.Check(UserName).IsNotNullOrWhiteSpace();
            return context.TryGetErrors(out errors);
        }
    }

    private interface IUpdateUserNameSession : IAsyncSession
    {
        Task<bool> CheckIfUserNameExistsAsync(string userName);
    }

    private sealed class UpdateUserNameSessionMock : AsyncSessionMock, IUpdateUserNameSession
    {
        public bool DoesUserNameExist { get; init; }

        public Task<bool> CheckIfUserNameExistsAsync(string userName) =>
            Task.FromResult(DoesUserNameExist);
    }
}