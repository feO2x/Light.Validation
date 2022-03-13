# Light.Validation
A lightweight library for validating incoming data in .NET HTTP services

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/blob/master/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-0.1.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Light.Validation/)
[![Documentation](https://img.shields.io/badge/Docs-Wiki-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/wiki)
[![Documentation](https://img.shields.io/badge/Docs-Changelog-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.Validation/releases)

## Light.Validation - easy and fast validation for HTTP services

- 👌 easy to start with, can be configured and extended
- 🏃‍♀️ fast in comparison
- 🔬 imperative API which can be easily debugged if needed
- ✉️ standard or custom error messages which can be translated
- 💠 easily integratable in different HTTP frameworks like ASP.NET Core MVC or Minimal APIs

## A simple example

Consider that you are writing an HTTP endpoint where a user can submit a rating for a movie. The incoming data might look like this:

```csharp
using System;

namespace MovieService;

public class RateMovieDto
{
    public Guid MovieId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
```

The `MovieId` identifies the target movie, the `Rating` is a value between 0 and 5 and `Comment` is an optional text that the user might add. You can now add a method to your DTO class that uses Light.Validation to validate it. Please be sure to add using statements for the `Light.Validation` and `Light.Validation.Checks` namespaces.

```csharp
using Light.Validation;
using Light.Validation.Checks;

namespace MovieService;

public class RateMovieDto
{
    public Guid MovieId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    
    public bool CheckForErrors([NotNullWhen(true)] Dictionary<string, object>? errors)
    {
        var context = new ValidationContext();
        
        // Perform simple checks that add the corresponding default
        // error message to the context if necessary
        context.Check(MovieId).IsNotEmpty();
        context.Check(Rating).IsIn(Range.FromInclusive(0).ToInclusive(5));
        
        
        // Light.Validation automatically normalizes strings (null -> empty string,
        // trim all other strings), which is why we reassign the value
        // to the property here. This behavior can be configured when creating the context.
        Comment = context.Check(Comment).IsShorterThan(100).Value;
        
        // Finally, check if any errors were found
        return context.TryGetErrors(out errors);
    }
}
```

You can then simply call this method in e.g. an ASP.NET Core MVC controller at the beginning of your action:

```csharp
[ApiController]
[Route("/api/movies/rate")]
[Authorize]
public class RateMovieController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RateMovie(RateMovieDto dto)
    {
        if (dto.CheckForErrors(out var errors))
            return BadRequest(errors); // or incorporate validation problem
            
         // Adding a movie rating to the database is ommitted for brevity's sake
    }
}
```

In a similar way, you can incorporate the validation mechanism in Minimal APIs:

```csharp
app.MapPost("/api/movies/rate", ([FromBody] RateMovieDto dto) =>
{
    if (dto.CheckForErrors(out var errors))
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
