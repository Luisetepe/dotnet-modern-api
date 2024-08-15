using GameStore.Api.Users.Connect.Features.Commands;

namespace GameStore.Api.Users.Features.Commands.CreateUser;

internal interface ICreateUserRepository
{
    Task<string> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
}