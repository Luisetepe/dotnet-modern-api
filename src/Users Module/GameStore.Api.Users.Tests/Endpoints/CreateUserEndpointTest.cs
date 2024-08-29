using GameStore.Api.Users.Connect.Features.Commands;
using GameStore.Api.Users.Endpoints;

namespace GameStore.Api.Users.Tests.Endpoints;

public class CreateUserEndpointTest(TestApp fixture) : TestBase<TestApp>
{
    [Fact]
    public async Task CreateUserEndpoint_ReturnsUserId()
    {
        var (response, client) = await fixture.Client.POSTAsync<CreateUserEndpoint, CreateUserCommand, CreateUserResponse>(new()
        {
            Name = "Test User",
            Email = "test.user@example.com",
            BirthDate = new DateOnly(1992, 3, 16),
            Address = new CreateUserCommand.UserAddress
            {
                Street = "123 Fake St",
                City = "Anytown",
                Country = "USA",
                ZipCode = "12345"
            }
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        client.UserId.Should().NotBeNullOrEmpty();
    }
}