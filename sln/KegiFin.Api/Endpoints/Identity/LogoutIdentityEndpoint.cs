using KegiFin.Api.Common.Api;
using KegiFin.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace KegiFin.Api.Endpoints.Identity;

public class LogoutIdentityEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandlerAsync).RequireAuthorization();

    private static async Task<IResult> HandlerAsync(SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}