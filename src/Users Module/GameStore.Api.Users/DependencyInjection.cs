using GameStore.Api.Users.Features.Commands.CreateUser;
using GameStore.Api.Users.Features.Queries.GetUser;
using GameStore.Api.Users.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserRepository, CreateUserRepository>();
        services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();
        services.AddScoped<IGetUserRepository, GetUserRepository>();
        
        return services;
    }
}