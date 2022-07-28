# Light.Validation
A lightweight library for validating incoming data in .NET HTTP services

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/blob/master/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-0.6.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Light.Validation/)
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

- **Package Reference in csproj**: `<PackageReference Include="Light.Validation" Version="0.6.0" />`
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
using Light.Validation;
using Light.Validation.Checks;

namespace MyWebApi;

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
app.MapPost("/api/movies/rate", (RateMovieDto? dto,
                                 RateMovieDtoValidator validator) =>
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

To inject your validator, you need to register it with your DI container. Light.Validation is designed to be fast, so by default, your validators can (and should be) registered as singletons:

```csharp
// The validation context factory only needs to be registered once
services.AddSingleton<IValidationContextFactory>(ValidationContextFactory.Instance) 
        .AddSingleton<RateMovieDtoValidator>();
```

## More advanced examples

### Validate query parameters

If you don't have a DTO, you can still use Light.Validation to validate parameters. This is usually the case in HTTP GET or DELETE endpoints that use query parameters. You can use the `ValidationContext` directly instead of creating a validator.

The following example shows an ASP.NET Core MVC controller that has a single HTTP GET action supporting paging and searching. 

```csharp
[ApiController]
[Route("/api/contacts")]
public class GetContactsController : ControllerBase
{
    public GetContactsController(ValidationContext validationContext,
                                 ISessionFactory<IGetContactsSession> sessionFactory)
    {
        ValidationContext = validationContext;
        SessionFactory = sessionFactory;
    }

    private ValidationContext ValidationContext { get; }
    private ISessionFactory<IGetContactsSession> SessionFactory { get; }

    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetContacts(int skip = 0,
                                                                  int take = 30,
                                                                  string? searchTerm = null)
    {
        if (ValidationContext.CheckForPagingErrors(skip, take, out var errors))
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

As you can see in the example above, an instance of `ValidationContext` is injected via the constructor of the controller. When the endpoint action `GetContacts` is called, the context is passed to the extension method `CheckForPagingErrors` to validate `skip` and `take`. To make this work, you need to register the `ValidationContext` class with your DI container. You should either use a transient or scoped lifetime (do not share a singleton instance of the validation context between HTTP requests, `ValidationContext` is neither thread-safe nor can it distinguish between the errors of several concurrent callers).

```csharp
services.AddTransient<ValidationContext>(_ => ValidationContextFactory.CreateContext());
```

This example shows that you can use the `ValidationContext` directly without implementing a validator. The `IValidationContextFactory` is the central point to configure how a `ValidationContext` instance is created. If you want to use the default one, simply use `ValidationContextFactory.Instance` for the `IValidationContextFactory` or `ValidationContextFactory.CreateContext()` to create the default context.

### Complex Child DTOs

Some DTO's are structured so that they contain a complex child DTO. You can use the `ValidateWith` extension method to validate child DTOs with another validator.

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

In the example above, the `NewContactDtoValidator` also takes a dependency on the `AddressDtoValidator`. The latter is then called in the former's `PerformValidation` method using `ValidateWith`. Both validators can be instantiated / registered with your DI container as singletons.

This example shows you that validators are composable. You can reuse validator when two or more DTOs use the same child DTO type.

**We highly recommend that you keep the structure of your DTOs as simple and as flat as possible. Do not introduce unnecessary nesting and/or collections in your DTOs.** Light.Validation will create a `ValidationContext` instance for each child DTO and each collection, which might increase GC pressure.

### Validating Collections

Sometimes a DTO contains a collection of values or other DTOs. You can use the `ValidateItems` extension method to validate the collection items:

```csharp
public sealed class Dto
{
    public List<int> Numbers { get; set; } = null!;
}

public sealed class DtoValidator : Validator<Dto>
{
    protected override Dto PerformValidation(ValidationContext context,
                                             Dto dto)
    {
        context.Check(dto.Numbers)
               .HasCount(3)
               .ValidateItems(number => number.IsIn(Range.FromInclusive(0).ToInclusive(5)));
        return dto;
    }
}
```

The above validator will ensure that the collection has exactly three numbers (with the call to `HasCount`) and that each number is between zero and five (with the call to `ValidateItems`). It's worth noting that the delegate you pass to `ValidateItems` is of the following form: `Check<T> ValidateItem(Check<T> item)`. This means that the `number` parameter in the example above is actually a `Check<int>` and within your delegate, you can check and transform the instance in the same way as you would do in a regular `PerformValidation` method of a validator.

Please note that you can also validate complex DTOs in collections, simply by calling `ValidateWith` within the delegate passed to `ValidateItems`:

```csharp
public class ContactDtoValidator : Validator<ContactDto>
{
    public NewContactDtoValidator(IValidationContextFactory factory,
                                  AddressDtoValidator childValidator)
        : base(factory)
    {
        ChildValidator = childValidator;
    }

    private AddressDtoValidator ChildValidator { get; }

    protected override ContactDto PerformValidation(ValidationContext context, ContactDto dto)
    {
        dto.Addresses
           .ValidateItems(address => address.ValidateWith(ChildValidator));
        return dto;
    }
}
```

**Again, we encourage you to keep your DTOs as simple and as flat as possible.** Do not introduce unnecessary nesting with child DTOs or collections.

### Async Validation

Most of your checks will probably be performed in-memory, but you might sometimes need to make an I/O call to validate a value. You can derive from the `AsyncValidator<T>` base class in these cases:

```csharp
public class LinkDtoValidator : AsyncValidator<LinkDto>, IDisposable
{
    public LinkDtoValidator(IValidationContextFactory validationContextfactory,
                            IHttpClientFactory httpClientFactory,
                            ILogger logger)
        : base(validationContextfactory)
    {
        HttpClientFactory = httpClientFactory;
        Logger = logger;
    }

    private HttpClient HttpClientFactory { get; }
    private ILogger Logger { get; }

    protected override async Task<LinkDto> PerformValidationAsync(ValidationContext context,
                                                                  LinkDto dto)
    {
        try
        {
            await using var httpClient = HttpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(dto.Url);
            if (!response.IsSuccessStatusCode)
                context.Check(dto.Url).AddError("The URL is not accessible");
        }
        catch(Exception exception)
        {
            Logger.Error(exception, "Could not validate {@Dto}", dto);
            context.Check(dto.Url).AddError("The URL could not be verified due to a connection error");
        }

        return dto;
    }
}
```

In the code example above, you see an async validator that takes the URL of a DTO and verifies that it is accessible via an HTTP request. You can also see that an error is created not only when the returned HTTP status code does not indicate success, but also when any connection error occurred. Of course, how you react to errors in async validation code is highly dependent on your context - it might e.g. also be fine to simply not catch the exception and produce an HTTP 500 Internal Server Error status code.

To use a validator in an endpoint, you register it with the DI container. Because we use an HTTP client in this example, we need to register it, too.

```csharp
// Don't forget to register the logger
services.AddSingleton<IValidationContextFactory>(ValidationContextFactory.Instance)
        .AddHttpClient()
        .AddSingleton<LinkDtoValidator>();
```

In your endpoints, you can then simply inject the validator, e.g. for Minimal APIs:

```csharp
app.MapPost("/api/validateUrl", async (LinkDto dto, LinkDtoValidator validator) => 
{
    var validationResult = await validator.ValidateAsync(dto);
    if (validationResult.TryGetErrors(out var errors))
        return Results.BadRequest(errors);
    
    return Results.Ok();
});
```

### Multi-Step Validation (Sharing ValidationContext)

Sometimes, DTO validation involves several steps, e.g. static checks and afterwards calling a third-party system. You can reuse a validation context in these cases.

```csharp
app.MapPut("/api/customers", async (UpdateCustomerDto dto,
                                    UpdateCustomerDtoValidator validator,
                                    ValidationContext validationContext,
                                    ISessionFactory<IUpdateCustomerSession> sessionFactory) => 
{
    if (validator.CheckForErrors(dto, validationContext, out var errors))
        return Results.BadRequest(errors);
    
    await using var session = await sessionFactory.OpenSessionAsync();
    var existingCustomer = await session.GetCustomerAsync(dto.Id);
    if (existingCustomer is null)
    {
        validationContext.Check(dto.Id).AddError($"There is no user with ID {dto.Id}");
        return Results.BadRequest(validationContext.Errors);
    }

    dto.UpdateCustomer(existingCustomer);
    await session.SaveChangesAsync();

    return Results.NoContent();
});
```

In the example above, the DTO is first validated with the validator. Afterwards, a database session is opened and the corresponding customer is loaded, after which the second validation takes place (if there actually is a customer with the specified ID). This is done by using a dedicated instance of `ValidationContext` that is also injected into the Minimal API endpoint. The validator will not create its own validation context in these circustances.

### Attaching Information to ValidationContext

You can attach any kind of data to a validation context. This might be useful when you want to pass information to a child validator:

```csharp
public class ParentValidator : Validator<CustomerDto>
{
    public ParentValidator(IValidationContextFactory factory,
                           ChildValidator childValidator)
        : base(factory)
    {
        ChildValidator = childValidator;
    }

    private ChildValidator ChildValidator { get; }

    protected override CustomerDto PerformValidation(ValidationContext context, CustomerDto dto)
    {
        // Remember, a Check<T> has an implicit conversion to the value it encapsulates.
        int customerId = context.Check(dto.Id).IsGreaterThan(0);
        context.SetAttachedObject("customerId", customerId);
        context.Check(dto.PaymentOption).ValidateWith(ChildValidator);
        return dto;
    }
}

public class ChildValidator : Validator<PaymentOptionDto>
{
    public ChildValidator(IValidationContextFactory factory, Func<IPaymentOptionsService> createPaymentOptionsService)
        : base(factory) 
    {
        CreatePaymentOptionsService = createPaymentOptionsService;
    }

    private Func<IPaymentOptionsService> CreatePaymentOptionsService { get; }

    protected override PaymentOptionDto PerformValidation(ValidationContext context, PaymentOptionDto dto)
    {
        // There is also a TryGetAttachedObject method
        var customerId = context.GetAttachedObject<int>("customerId");
        using var paymentOptionsService = CreatePaymentOptionsService();
        if (!paymentOptionsService.CheckIfCustomerIsEligible(customerId, dto.PaymentDetails))
            context.Check(dto.PaymentDetails).AddError($"The customer with ID \"{customerId}\" cannot use this payment method");
        return dto;
    }
}
```

In the example above, the parent validator attaches the `customerId` to the validation context by calling `SetAttachedObject`. When the child validator is called, it retrieves this value using the `GetAttachedObject<T>` method.
