using GameStore.Api.Users.Connect.Features.Queries;
using LanguageExt;

namespace GameStore.Api.Users.Features.Queries.GetUser;

public interface IGetUserRepository
{
    Task<Option<GetUserResponse>> GetUserById(Ulid id, CancellationToken ct = default);
}