namespace Light.Validation.Benchmarks;

public sealed class LightValidator : Validator<UpdateUserNameDto>
{
    protected override void CheckForErrors(ValidationContext context, UpdateUserNameDto dto)
    {
        context.Check(dto.Id).GreaterThan(0);
        dto.UserName = context.Check(dto.UserName).TrimAndCheckNotWhiteSpace();
    }
}