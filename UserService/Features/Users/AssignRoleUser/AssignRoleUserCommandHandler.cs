using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using MediatR;
using Microsoft.Extensions.Options;
using OrderDispatch.Contracts.Constants;
using UserService.Models.Enums;
using UserService.Options;

namespace UserService.Features.Users.AssignRoleUser;

public class AssignRoleUserCommandHandler(
    KeycloakClient keycloakClient,
    IOptions<KeycloakSettings> keycloakOptions) : IRequestHandler<AssignRoleUserCommand, AssignRoleUserResult>
{
    public async Task<AssignRoleUserResult> Handle(AssignRoleUserCommand request, CancellationToken cancellationToken)
    {
        User user;
        try
        {
            user = await keycloakClient.GetUserAsync(keycloakOptions.Value.Realm, request.AssignRoleUserId,
                cancellationToken);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            return AssignRoleUserResult.NotFound;
        }

        var currentUser =
            await keycloakClient.GetUserAsync(keycloakOptions.Value.Realm, request.UserId, cancellationToken);

        if (!(AppRoles.IsHigher(currentUser.RealmRoles.First(), user.RealmRoles.First()) &&
              AppRoles.IsHigher(currentUser.RealmRoles.First(), request.Role)))
            return AssignRoleUserResult.CannotAssignRole;

        var role = await keycloakClient.GetRoleByNameAsync(keycloakOptions.Value.Realm, request.Role,
            cancellationToken);

        await keycloakClient.AddRealmRoleMappingsToUserAsync(keycloakOptions.Value.Realm, user.Id, [role],
            cancellationToken);
        return AssignRoleUserResult.AssignRole;
    }
}