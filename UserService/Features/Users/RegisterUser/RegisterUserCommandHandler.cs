using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using MediatR;
using Microsoft.Extensions.Options;
using UserService.Options;

namespace UserService.Features.Users.RegisterUser;

public class RegisterUserCommandHandler(
    KeycloakClient keycloakClient,
    IOptions<KeycloakSettings> keycloakOptions) : IRequestHandler<RegisterUserCommand, User?>
{
    public async Task<User?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Enabled = true,
            EmailVerified = true,
            Credentials =
            [
                new Credentials
                {
                    Type = "password",
                    Value = request.Password,
                    Temporary = false
                }
            ]
        };

        try
        {
            await keycloakClient.CreateUserAsync(keycloakOptions.Value.Realm, user, cancellationToken);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 409)
        {
            return null;
        }

        var createdUser = (await keycloakClient.GetUsersAsync(keycloakOptions.Value.Realm, username: request.UserName,
            cancellationToken: cancellationToken)).First();
        return createdUser;
    }
}