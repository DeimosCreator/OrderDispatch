using OrderDispatch.Contracts.Constants;

namespace OrderDispatch.Tests;

public class AppRolesTests
{
    [Test]
    public void IsHigher_WhenCallerHasHigherRank_ShouldReturnTrue()
    {
        var result = AppRoles.IsHigher([AppRoles.CompanyOwner], [AppRoles.BranchManager]);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsHigher_WhenCallerHasEqualOrLowerRank_ShouldReturnFalse()
    {
        Assert.That(AppRoles.IsHigher([AppRoles.Chef], [AppRoles.Chef]), Is.False);
        Assert.That(AppRoles.IsHigher([AppRoles.Chef], [AppRoles.BranchManager]), Is.False);
    }

    [Test]
    public void IsHigher_ShouldIgnoreSystemRolesAndUseMaxRank()
    {
        string[] callerRoles = ["offline_access", "default-roles-orderdispatchrealm", AppRoles.SuperAdmin];
        string[] targetRoles = [AppRoles.CompanyOwner];

        Assert.That(AppRoles.IsHigher(callerRoles, targetRoles), Is.True);
    }

    [Test]
    public void GetFilteredRoles_ShouldExcludeUnknownAndSystemRoles()
    {
        string[] roles = ["offline_access", "uma_authorization", AppRoles.Chef, AppRoles.Customer];

        var filtered = AppRoles.GetFilteredRoles(roles).ToArray();

        Assert.That(filtered, Is.EqualTo(new[] { AppRoles.Chef, AppRoles.Customer }));
    }
}