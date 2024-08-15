using GameStore.Api.SharedKernel.Exceptions;
using GameStore.Api.Users.Connect.Features.Queries;
using LanguageExt.Common;
using MediatR;

namespace GameStore.Api.Users.Features.Queries.GetUser;

internal class GetUserHandler(IGetUserRepository repository) : IRequestHandler<GetUserQuery, Result<GetUserResponse>>
{
    public async Task<Result<GetUserResponse>> Handle(GetUserQuery request, CancellationToken ct)
    {
        var userId = Ulid.Parse(request.Id);
        var result = await repository.GetUserById(userId, ct);

        return result.Match(
            user => user,
            () => new Result<GetUserResponse>(new NotFoundException($"User with id {userId} not found"))
        );
    }
}