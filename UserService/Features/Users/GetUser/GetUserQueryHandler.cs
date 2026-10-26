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
        var currentUserRole = (await keycloakClient.GetRealmRoleMappingsForUserAsync(keycloakOptions.Value.Realm,
            request.UserId, cancellationToken)).Select(role => role.Name.ToString());
        
        var userRole = (await keycloakClient.GetRealmRoleMappingsForUserAsync(keycloakOptions.Value.Realm,
            request.Id, cancellationToken)).Select(role => role.Name.ToString());

        if (request.Id != request.UserId &&
            !AppRoles.IsHigher(currentUserRole, userRole)) return null;

        var user = await keycloakClient.GetUserAsync(keycloakOptions.Value.Realm, request.Id, cancellationToken);
        return user;
    }
}