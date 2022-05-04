# Light.Validation
A lightweight library for validating incoming data in .NET HTTP services

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/blob/master/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-0.5.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Light.Validation/)
[![Documentation](https://img.shields.io/badge/Docs-Wiki-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/wiki)
[![Documentation](https://img.shields.io/badge/Docs-Changelog-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/releases)

## Light.Validation - easy and fast validation for HTTP services

- 👌 easy to start with, can be configured and extended
- 🚀 fast in comparison
- 🔬 imperative API which can be easily debugged if needed
- 👓 supports C# Nullable Reference Types
- ✉️ use the standard error messages or customize / translate them to your liking
- 💠 can be easily integrated in different HTTP frameworks like ASP.NET Core MVC or Minimal APIs

## How to install

Light.Validation is built against .NET 6 and .NET Standard 2.0 and available on [NuGet](https://www.nuget.org/packages/Light.Validation/).

- **Package Reference in csproj**: `<PackageReference Include="Light.Validation" Version="0.5.0" />`
- **dotnet CLI**: `dotnet add package Light.Validation`
- **Visual Studio Package Manager Console**: `Install-Package Light.Validation`

## A simple example

Consider that you are writing an HTTP endpoint where a user can submit a rating for a movie. The incoming data might look like this:

```csharp
public class RateMovieDto
{
    public Guid MovieId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
```

The `MovieId` identifies the target movie, the `Rating` is a value between 0 and 5 and `Comment` is an optional text that the user might add. To validate this DTO, you should create a validator. Please be sure to add using statements for the `Light.Validation` and `Light.Validation.Checks` namespaces.

```csharp
public class RateMovieDtoValidator : Validator<RateMovieDto>
{
    // The passed factory is used to create instances of ValidationContext
    public RateMovieDtoValidator(IValidationContextFactory factory)
        : base(factory) { }

    // By default, a validator will check that the dto is not null
    // before calling this method. Thus in this method, you can
    // simply dereference your DTO without the fear of causing
    // a NullReferenceException.
    protected override RateMovieDto PerformValidation(ValidationContext context,
                                                      RateMovieDto dto)
    {
        // Perform simple checks that add the corresponding default
        // error message to the context if necessary
        context.Check(dto.MovieId).IsNotEmpty();
        context.Check(dto.Rating).IsIn(Range.FromInclusive(0).ToInclusive(5));

        // Light.Validation automatically normalizes strings
        // (null -> empty string, other strings -> trim). We reassign
        // the normalized value to the property here, so that it is
        // available to other code that processes the DTO.
        dto.Comment = context.Check(Comment).IsShorterThan(100);

        // Usually, you want to return the passed DTO, but you can also
        // transform (normalize) the DTO and return another instance
        // if you want to.
        return dto;
    }
}
```

You can then simply inject and use the validator in e.g. an ASP.NET Core MVC controller:

```csharp
[ApiController]
[Route("/api/movies/rate")]
public class RateMovieController : ControllerBase
{
    public RateMoveController(RateMovieDtoValidator validator) =>
         Validator = validator;

    private RateMovieValidator Validator { get; }

    [HttpPost]
    public async Task<IActionResult> RateMovie(RateMovieDto? dto)
    {
        // Validators support C# NRTs, thus the dto variable
        // is considered not null by the C# compiler when
        // CheckForErrors returns false
        if (Validator.CheckForErrors(dto, out object? errors))
            return BadRequest(errors);
            
        // Adding a movie rating to the database is ommitted for brevity's sake
    }
}
```

In a similar way, you can incorporate the validation mechanism in Minimal APIs:

```csharp
app.MapPost("/api/movies/rate", ([FromBody] RateMovieDto? dto,
                                 [FromServices] RateMovieDtoValidator validator)) =>
{
    if (Validator.CheckForErrors(dto, out var errors))
        return Results.BadRequest(errors);
        
    // Adding a movie rating to the database is ommitted for brevity's sake
});
```

The resulting body of the bad request would then look like the following JSON document (in case that all checks found errors):

```json
{
    "movieId": "movieId must not be an empty GUID",
    "rating": "rating must be in range from 0 (inclusive) to 5 (inclusive)",
    "comment": "comment must be shorter than 100"
}
```

To inject your validator, you need to register it with your DI container. Light.Validation is designed to be fast, so by default, your validators can and should be registered as singletons:

```csharp
// The validation context factory only needs to be registered once
services.AddSingleton<IValidationContextFactory>(ValidationContextFactory.Instance) 
        .AddSingleton<RateMovieDtoValidator>();
```

## More advanced examples

### Validate query parameters

If you don't have a DTO, you can still use Light.Validation to validate parameters. This is usually the case in HTTP GET or DELETE endpoints that use query parameters. You can use the `IValidationContextFactory` directly instead of creating a validator.

The following example shows a an ASP.NET Core MVC controller that has a single HTTP GET action supporting paging and searching. 

```csharp
[ApiController]
[Route("/api/contacts")]
public class GetContactsController : ControllerBase
{
    public GetContactsController(IValidationContextFactory validationFactory,
                                 ISessionFactory<IGetContactsSession> sessionFactory)
    {
        ValidationFactory = validationFactory;
        SessionFactory = sessionFactory;
    }

    private IValidationContextFactory ValidationFactory { get; }
    private ISessionFactory<IGetContactsSession> SessionFactory { get; }

    public async Task<ActionResult<List<ContactDto>>> GetContacts(int skip = 0,
                                                                  int take = 30,
                                                                  string? searchTerm = null)
    {
        if (ValidationFactory.CreateContext().CheckForPagingErrors(skip, take, out var errors))
            return BadRequest(errors);

        await using var session = await SessionFactory.OpenSessionAsync();
        var contacts = session.GetContactsAsync(skip, take, searchTerm);
        return ContactDto.FromContacts(contacts);
    }
}

public static class ValidationExtensions
{
    public static bool CheckForPagingErrors(this ValidationContext context,
                                            int skip,
                                            int take,
                                            [NotNullWhen(true)] out object? errors)
    {
        context.Check(skip).IsGreaterThanOrEqualTo(0);
        context.Check(take).IsIn(Range.FromInclusive(1).ToInclusive(100));
        return context.TryGetErrors(out errors);
    }
}
```

As you can see in the example above, an instance of `IValidationContextFactory` is injection via the constructor of the controller. When the endpoint action `GetContacts` is called, the factory is used to create a validation context which in turn is passed to the extension method `CheckForPagingErrors` to validate `skip` and `take`.

This example shows that you can use the `ValidationContext` directly without implementing a validator. The `IValidationContextFactory` is the central point to configure how a `ValidationContext` instance is created. If you want to use the default one, simply use `ValidationContextFactory.Instance`.

### Complex Child DTOs

Some DTO's are structured so that they contain a complex child DTO. You can use the `ValidateWith` to validate child DTOs within a validator.

```csharp
public class NewContactDto
{
    public string Name { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = null!;
}

public class AddressDto
{
    public string Street { get; set; }
    public string Location { get; set; }
}

public class NewContactDtoValidator : Validator<NewContactDto>
{
    public NewContactDtoValidator(IValidationContextFactory factory,
                                  AddressDtoValidator childValidator)
        : base(factory)
    {
        ChildValidator = childValidator;
    }

    private AddressDtoValidator ChildValidator { get; }

    protected override NewContactDto PerformValidation(ValidationContext context, NewContactDto dto)
    {
        dto.Name = context.Check(dto.Name).HasLengthIn(Range.FromInclusive(2).ToInclusive(100));
        dto.Address = context.Check(dto.Address).ValidateWith(ChildValidator);
        return dto;
    }
}

public class AddressDtoValidator : Validator<AddressDto>
{
    public AddressDtoValidator(IValidationContextFactory factory)
        : base(factory) { }

    protected override AddressDto PerformValidation(ValidationContext context, AddressDto dto)
    {
        dto.Street = context.Check(dto.Street).HasLengthIn(Range.FromInclusive(2).ToInclusive(100));
        dto.Location = context.Check(dto.Location).HasLengthIn(Range.FromInclusive(2).ToInclusive(100));
        return dto;
    }
}
```

In the above example, the `NewContactDtoValidator` also takes a dependency on the `AddressDtoValidator`. The latter is then called in the former's `PerformValidation` method using `ValidateWith`. Both validators can be instantiated / registered with your DI container as singletons.

This example shows you that validators are composable. You can reuse validator when two or more DTOs use the same child DTOs.