using Keycloak.Net;
using Keycloak.Net.Models.Users;
using MediatR;
using Microsoft.Extensions.Options;
using OrderDispatch.Contracts.Constants;
using UserService.Options;

namespace UserService.Features.Users.GetUser;

public class GetUserQueryHandler(
    KeycloakClient keycloakClient,
    IOptions<KeycloakSettings> keycloakOptions) : IRequestHandler<GetUserQuery, User?>
{
    public async Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser =
            await keycloakClient.GetUserAsync(keycloakOptions.Value.Realm, request.UserId, cancellationToken);
        var user = await keycloakClient.GetUserAsync(keycloakOptions.Value.Realm, request.Id, cancellationToken);

        if (currentUser.Id != user.Id &&
            !AppRoles.IsHigher(currentUser.RealmRoles.First(), user.RealmRoles.First())) return null;

        return user;
    }
}