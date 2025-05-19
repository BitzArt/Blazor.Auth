using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Tests;

public class BlazorAuthAuthenticationStateProviderTests
{
    private class TestUserService : IUserService
    {
        private int _counter = 0;

        public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(200, cancellationToken);

            var claim = new Claim("counter", (++_counter).ToString());
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity([claim])));
        }

        public Task<AuthenticationOperationInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task SignOutAsync(CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_Simaltaneous_ShouldReuseTheSameRequest()
    {
        // Arrange
        var provider = new BlazorAuthAuthenticationStateProvider(new TestUserService());

        // Act
        var primaryTask = provider.GetAuthenticationStateAsync();

        var secondaryTasks = new List<Task<AuthenticationState>>();

        for (int i = 0; i < 10; i++)
        {
            secondaryTasks.Add(provider.GetAuthenticationStateAsync());
            await Task.Delay(10);
        }

        // Assert
        var result = await primaryTask;

        Assert.NotNull(result);
        var claim = result.User.Claims.First();
        Assert.NotNull(claim);
        Assert.Equal("counter", claim.Type);
        Assert.Equal("1", claim.Value);

        var secondaryResults = await Task.WhenAll(secondaryTasks);
        Assert.All(secondaryResults, x =>
        {
            Assert.True(x == result);
        });
    }
}
