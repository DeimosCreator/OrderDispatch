using Flurl.Http;
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
        var realm = keycloakOptions.Value.Realm;

        try
        {
            if (request.Id == request.UserId)
                return await keycloakClient.GetUserAsync(realm, request.Id, cancellationToken);
            
            var currentUserRole = (await keycloakClient.GetRealmRoleMappingsForUserAsync(
                realm, request.UserId, cancellationToken)).Select(role => role.Name);

            var userRole = (await keycloakClient.GetRealmRoleMappingsForUserAsync(
                realm, request.Id, cancellationToken)).Select(role => role.Name);

            if (!AppRoles.IsHigher(currentUserRole, userRole))
                return null;

            return await keycloakClient.GetUserAsync(realm, request.Id, cancellationToken);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response?.StatusCode == 404)
        {
            return null;
        }
    }
}