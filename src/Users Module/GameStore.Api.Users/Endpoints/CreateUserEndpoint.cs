using FastEndpoints;
using GameStore.Api.SharedKernel.Extensions;
using GameStore.Api.Users.Connect.Features.Commands;
using MediatR;

namespace GameStore.Api.Users.Endpoints;

internal class CreateUserEndpoint(ISender mediator) : Endpoint<CreateUserCommand, CreateUserResponse>
{
    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        
        await result.Match(
            response => SendAsync(response, cancellation: ct),
            error => SendResultAsync(error.MapToApiResult())
        );
    }
}

internal class CreateUserEndpointSwagger : Summary<CreateUserEndpoint>
{
    public CreateUserEndpointSwagger()
    {
        Summary = "Create a new user";
        Description = "Create a new user";
        ExampleRequest = new CreateUserCommand
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            Address = new CreateUserCommand.UserAddress
            {
                Street = "123 Main St",
                City = "Anytown",
                Country = "USA",
                ZipCode = "12345"
            }
        };
        Response(
            200,
            "Returns the newly created user identifier.",
            example: new CreateUserResponse
            {
                UserId = Ulid.NewUlid().ToString()
            }
        );
        Response(400, "One or more validation errors occurred.", example: new
        {
            Message = "One or more validation errors occurred.",
            Errors = new Dictionary<string, string[]>
            {
                ["Name"] = ["Name must be at least 3 characters long."],
                ["Email"] = ["Email must be a valid email address."],
                ["Address.Street"] = ["Street must be at least 1 character long."],
                ["Address.City"] = ["City must be at least 1 character long."],
                ["Address.Country"] = ["Country must be at least 1 character long."],
                ["Address.ZipCode"] = ["ZipCode must be at least 1 character long."],
                ["BirthDate"] = ["Birth date cannot be in the future."]
            }
        });
        Response(
            500,
            "An error occurred while creating the user.",
            example: new
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Title = "Something went wrong.",
                Status = 500,
                Detail = "An error occurred while creating the user.",
                    
            }
        );
    }
}