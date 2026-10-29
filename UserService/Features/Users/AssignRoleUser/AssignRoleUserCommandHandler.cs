using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Messaging.Events.Users;
using UserService.Models.Enums;
using UserService.Options;

namespace UserService.Features.Users.AssignRoleUser;

public class AssignRoleUserCommandHandler(
    KeycloakClient keycloakClient,
    IOptions<KeycloakSettings> keycloakOptions,
    IPublishEndpoint publishEndpoint) : IRequestHandler<AssignRoleUserCommand, AssignRoleUserResult>
{
    public async Task<AssignRoleUserResult> Handle(AssignRoleUserCommand request, CancellationToken cancellationToken)
    {
        var realm = keycloakOptions.Value.Realm;

        Role[] callerRoleObjects;
        Role[] targetUserRoleObjects;

        try
        {
            callerRoleObjects =
                (await keycloakClient.GetRealmRoleMappingsForUserAsync(realm, request.UserId, cancellationToken))
                .ToArray();
            targetUserRoleObjects =
                (await keycloakClient.GetRealmRoleMappingsForUserAsync(realm, request.AssignRoleUserId,
                    cancellationToken)).ToArray();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            return AssignRoleUserResult.NotFound;
        }

        var currentUserRoles = callerRoleObjects.Select(r => r.Name).ToArray();
        var targetUserRoles = targetUserRoleObjects.Select(r => r.Name);

        if (!AppRoles.IsHigher(currentUserRoles, targetUserRoles) ||
            !AppRoles.IsHigher(currentUserRoles, [request.Role]))
            return AssignRoleUserResult.CannotAssignRole;

        Role newRole;
        try
        {
            newRole = await keycloakClient.GetRoleByNameAsync(realm, request.Role, cancellationToken);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            return AssignRoleUserResult.CannotAssignRole;
        }

        var rolesToDelete = targetUserRoleObjects
            .Where(r => AppRoles.GetFilteredRoles([r.Name]).Any())
            .ToArray();

        if (rolesToDelete.Length > 0)
            await keycloakClient.DeleteRealmRoleMappingsFromUserAsync(realm, request.AssignRoleUserId, rolesToDelete,
                cancellationToken);

        await keycloakClient.AddRealmRoleMappingsToUserAsync(realm, request.AssignRoleUserId, [newRole],
            cancellationToken);

        await publishEndpoint.Publish(new UserRoleChangedEvent(request.AssignRoleUserId, newRole.Name),
            cancellationToken);

        return AssignRoleUserResult.AssignRole;
    }
}