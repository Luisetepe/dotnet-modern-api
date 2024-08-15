using Dapper;
using GameStore.Api.Users.Connect.Features.Queries;
using LanguageExt;
using Npgsql;

namespace GameStore.Api.Users.Features.Queries.GetUser;

internal class GetUserRepository(NpgsqlConnection connection) : IGetUserRepository
{
    public async Task<Option<GetUserResponse>> GetUserById(Ulid id, CancellationToken ct)
    {
        var queryParams = new { Id = id.ToString() };
        var result = await connection.QuerySingleOrDefaultAsync
        <
            (string UserId, string Name, string Email, DateOnly BirthDate, string UserAddressId, string Street, string City, string Country, string ZipCode)?
        >
        (
            """
            SELECT      u.id as UserId, 
                        name as Name, 
                        email as Email, 
                        birth_date as BirthDate, 
                        ua.id as UserAddressId, 
                        street as Street, 
                        city as City, 
                        country as Country, 
                        zip_code as ZipCode
            FROM        "user".users u 
            INNER JOIN  "user".user_addresses ua ON u.id = ua.user_id
            WHERE       u.id = @Id
            """,
            queryParams
        );
        
        if (result is null)
        {
            return null;
        }

        return new GetUserResponse
        {
            Id = result.Value.UserId,
            Name = result.Value.Name,
            Email = result.Value.Email,
            BirthDate = result.Value.BirthDate,
            Address = new GetUserResponse.UserAddress
            {
                Id = result.Value.UserAddressId,
                Street = result.Value.Street,
                City = result.Value.City,
                Country = result.Value.Country,
                ZipCode = result.Value.ZipCode
            }
        };
    }
    
    
}

