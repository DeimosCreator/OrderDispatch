using Microsoft.AspNetCore.Authorization;
using OrderDispatch.Contracts.Constants;

namespace OrderDispatch.Contracts.Security;

public class AuthorizeMinRankAttribute(string rank)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public int MinRank { get; } = AppRoles.GetRank(rank);

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}