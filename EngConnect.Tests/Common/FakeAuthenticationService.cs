using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EngConnect.Tests.Common;

internal sealed class FakeAuthenticationService : IAuthenticationService
{
    private readonly ClaimsPrincipal _principal;

    public FakeAuthenticationService(ClaimsPrincipal principal)
    {
        _principal = principal;
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        var ticket = new AuthenticationTicket(_principal, scheme ?? "TestAuth");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }
}
