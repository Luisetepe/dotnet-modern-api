using System.Data;
using Dapper;
using GameStore.Api.Users.Connect.Features.Commands;
using Npgsql;

namespace GameStore.Api.Users.Features.Commands.CreateUser;

internal class CreateUserRepository(NpgsqlConnection dbConnection) : ICreateUserRepository
{
    public async Task<string> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var newUserId = Ulid.NewUlid().ToString();
        var newAddressId = Ulid.NewUlid().ToString();
        
        await dbConnection.ExecuteAsync(
            """
            INSERT INTO "user".users (id, name, email, birth_date)
            VALUES      (@Id, @Name, @Email, @BirthDate)
            """,
            new
            {
                Id = newUserId,
                command.Name,
                command.Email,
                command.BirthDate
            });
        
        await dbConnection.ExecuteAsync(
            """
            INSERT INTO "user".user_addresses (id, user_id, street, city, country, zip_code) 
            VALUES      (@Id, @UserId, @Street, @City, @Country, @ZipCode)
            """,
            new
            {
                Id = newAddressId,
                UserId = newUserId,
                command.Address.Street, 
                command.Address.City, 
                command.Address.Country,
                command.Address.ZipCode
            });
        
        return newUserId;
    }
}