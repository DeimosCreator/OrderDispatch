using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using MassTransit;
using Microsoft.Extensions.Options;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Messaging.Commands;
using UserService.Models.Enums;
using UserService.Options;

namespace UserService.Features.Users.Consumers;

public class GetUserRolesConsumer(
    KeycloakClient keycloakClient,
    IOptions<KeycloakSettings> keycloakOptions) : IConsumer<GetUserRolesRequest>
{
    public async Task Consume(ConsumeContext<GetUserRolesRequest> context)
    {
        IEnumerable<Role> roleMappings;
        try
        {
            roleMappings = await keycloakClient.GetRealmRoleMappingsForUserAsync(
                keycloakOptions.Value.Realm,
                context.Message.UserId,
                context.CancellationToken);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            await context.RespondAsync(new UserRoleResponse(null));
            return;
        }

        var roles = roleMappings.Select(r => r.Name).ToArray();
        var role = AppRoles.GetMaxRole(roles);

        await context.RespondAsync(new UserRoleResponse(role));
    }
}