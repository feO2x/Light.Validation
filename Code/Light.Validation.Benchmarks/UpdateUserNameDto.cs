﻿using System.Collections.Generic;
using Light.Validation.Checks;

namespace Light.Validation.Benchmarks;

public sealed class UpdateUserNameDto
{
    public int Id { get; init; }
    public string UserName { get; set; } = string.Empty;

    public bool CheckForErrors(out object? errors)
    {
        var context = ValidationContextFactory.CreateContext();
        context.Check(Id).IsGreaterThan(0);
        UserName = context.Check(UserName)
                          .IsNotNullOrWhiteSpace();
        return context.TryGetErrors(out errors);
    }
}