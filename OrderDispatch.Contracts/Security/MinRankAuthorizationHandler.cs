using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using OrderDispatch.Contracts.Constants;

namespace OrderDispatch.Contracts.Security;

public class MinRankAuthorizationHandler : AuthorizationHandler<AuthorizeMinRankAttribute>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthorizeMinRankAttribute requirement)
    {
        var userRoles = context.User.Claims
            .Where(c => c.Type is ClaimTypes.Role or "role")
            .Select(c => c.Value)
            .ToList();

        var realmAccessClaim = context.User.FindFirst("realm_access")?.Value;
        if (!string.IsNullOrEmpty(realmAccessClaim))
            try
            {
                using var doc = JsonDocument.Parse(realmAccessClaim);
                if (doc.RootElement.TryGetProperty("roles", out var rolesElement))
                    foreach (var role in rolesElement.EnumerateArray())
                        if (role.GetString() is { } roleName)
                            userRoles.Add(roleName);
            }
            catch
            {
                // ignored
            }

        var currentRank = AppRoles.GetMaxRank(userRoles);

        if (currentRank >= requirement.MinRank)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}