using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tools;

namespace Light.Validation.Benchmarks;

public sealed class UpdateUserNameDto
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