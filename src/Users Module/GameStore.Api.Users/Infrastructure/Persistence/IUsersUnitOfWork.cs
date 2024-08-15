namespace GameStore.Api.Users.Infrastructure.Persistence;

internal interface IUsersUnitOfWork
{
    Task<TRepository> CreateRepositoryAsync<TRepository>(CancellationToken cancellationToken = default) where TRepository : class;

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);
}