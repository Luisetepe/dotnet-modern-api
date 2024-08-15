using FastEndpoints;
using GameStore.Api.SharedKernel.Extensions;
using GameStore.Api.Users.Connect.Features.Queries;
using MediatR;

namespace GameStore.Api.Users.Endpoints;

internal class GetUserEndpoint(ISender mediator) : Endpoint<GetUserQuery, GetUserResponse>
{
    public override void Configure()
    {
        Get("/api/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        
        await result.Match(
            response => SendAsync(response, cancellation: ct),
            error  => SendResultAsync(error.MapToApiResult())
        );
    }
}

internal class GetUserEndpointSwagger : Summary<GetUserEndpoint>
{
    public GetUserEndpointSwagger()
    {
        Summary = "Retreives a user by id";
        Description = "Retreives a user by id";
        ExampleRequest = new GetUserQuery
        {
            Id = Ulid.NewUlid().ToString()
        };
        Response(
            200,
            "Returns the found user.",
            example: new GetUserResponse
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                Address = new GetUserResponse.UserAddress
                {
                    Id = Ulid.NewUlid().ToString(),
                    Street = "123 Main St",
                    City = "Anytown",
                    Country = "USA",
                    ZipCode = "12345"
                }
            }
        );
        Response(404, "The user with the specified id was not found.", example: new
        {
            Message = $"User with id {Ulid.NewUlid().ToString()} not found."
        });
        Response(
            500,
            "An error occurred while retreiving the user.",
            example: new
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Title = "Something went wrong.",
                Status = 500,
                Detail = "An error occurred while retreiving the user.",
                    
            }
        );
    }
}