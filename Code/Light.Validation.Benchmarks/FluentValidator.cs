using FluentValidation;
using Result = FluentValidation.Results.ValidationResult;

namespace Light.Validation.Benchmarks;

public sealed class FluentValidator : AbstractValidator<UpdateUserNameDto>
{
    public FluentValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0);
        RuleFor(dto => dto.UserName).NotEmpty();
    }

    protected override bool PreValidate(ValidationContext<UpdateUserNameDto> context, Result result)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier -- DTO value can actually be null
        context.InstanceToValidate.UserName = context.InstanceToValidate.UserName?.Trim() ?? string.Empty;
        return true;
    }
}