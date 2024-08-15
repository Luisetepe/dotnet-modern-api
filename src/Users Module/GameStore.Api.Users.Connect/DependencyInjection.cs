using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Users.Connect;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersConnect(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(UsersConnectAssembly.Instance);
        
        return services;
    }
}