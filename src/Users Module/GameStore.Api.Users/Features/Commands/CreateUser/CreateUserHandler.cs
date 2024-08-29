using GameStore.Api.Users.Connect.Features.Commands;
using GameStore.Api.Users.Infrastructure.Services.Interfaces;
using LanguageExt.Common;
using MediatR;

namespace GameStore.Api.Users.Features.Commands.CreateUser;

internal class CreateUserHandler(IUsersUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var repo = await unitOfWork.CreateRepositoryAsync<ICreateUserRepository>(cancellationToken);
        var newUserId = await repo.CreateUserAsync(request, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateUserResponse
        {
            UserId = newUserId
        };
    }
}