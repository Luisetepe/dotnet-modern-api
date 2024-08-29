using GameStore.Api.Users.Connect.Features.Queries;
using GameStore.Api.Users.Endpoints;

namespace GameStore.Api.Users.Tests.Endpoints;

public class GetUserEndpointTests(TestApp fixture) : TestBase<TestApp>
{
    [Fact]
    public async Task GetUserEndpoint_ReturnsUser()
    {
        var (response, client) =
            await fixture.Client.GETAsync<GetUserEndpoint, GetUserQuery, GetUserResponse>(new()
                { Id = "01J5APKNJXTPTBND8GA59GBWCH" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        client.Id.Should().Be("01J5APKNJXTPTBND8GA59GBWCH");
        client.Name.Should().Be("John Doe");
        client.Email.Should().Be("john.doe@example.com");
        client.BirthDate.Should().Be(new DateOnly(1992, 3, 16));
        client.Address.Id.Should().Be("01J5APKNJXHV6RQTHV10ZEBA6A");
        client.Address.Street.Should().Be("123 Main St");
    }
}