using System.Data;
using System.Reflection;
using Npgsql;

namespace GameStore.Api.Users.Infrastructure.Persistence;

internal class UsersUnitOfWork(NpgsqlDataSource dataSource) : IUsersUnitOfWork, IDisposable
{
    private static readonly Type[] RepositoryTypes = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(x => x is { IsClass: true, IsInterface: false } && x.Name.EndsWith("Repository"))
        .ToArray();

    private NpgsqlTransaction? _dbTransaction;
    private bool _disposed = false;
    private NpgsqlConnection? _dbConnection;

    public async Task<TRepository> CreateRepositoryAsync<TRepository>(CancellationToken cancellationToken = default)
        where TRepository : class
    {
        _dbConnection ??= await dataSource.OpenConnectionAsync(cancellationToken);
        
        var repo = RepositoryTypes.Single(x => x.IsAssignableTo(typeof(TRepository)));
        var instance = Activator.CreateInstance(repo, _dbConnection);
        
        if (instance is null) throw new Exception("Failed to create repository instance for repository type " + repo.Name);
        
        return (TRepository)instance;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _dbConnection ??= await dataSource.OpenConnectionAsync(cancellationToken);
        _dbTransaction = await _dbConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_dbConnection);
        ArgumentNullException.ThrowIfNull(_dbTransaction);

        await _dbTransaction.RollbackAsync(cancellationToken);
        await _dbConnection.CloseAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_dbConnection);
        ArgumentNullException.ThrowIfNull(_dbTransaction);

        await _dbTransaction.CommitAsync(cancellationToken);
        await _dbConnection.CloseAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbTransaction?.Dispose();
                _dbConnection?.Dispose();
            }
        }

        _disposed = true;
    }
}