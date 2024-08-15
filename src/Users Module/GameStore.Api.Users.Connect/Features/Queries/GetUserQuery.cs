using LanguageExt.Common;
using MediatR;

namespace GameStore.Api.Users.Connect.Features.Queries;

public record GetUserResponse
{
    public record UserAddress
    {
        public string Id { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }
    }
    
    public string Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public DateOnly BirthDate { get; init; }
    public UserAddress Address { get; init; }
}

public record GetUserQuery : IRequest<Result<GetUserResponse>>
{
    public string Id { get; init; }
}