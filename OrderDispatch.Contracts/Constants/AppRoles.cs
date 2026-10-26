namespace OrderDispatch.Contracts.Constants;

public static class AppRoles
{
    public const string SuperAdmin = "super-admin";
    public const string CompanyOwner = "company-owner";
    public const string BranchManager = "branch-manager";
    public const string Chef = "chef";
    public const string Courier = "courier";
    public const string Customer = "customer";

    private static int GetRank(string role) => role switch
    {
        SuperAdmin => 6,
        CompanyOwner => 5,
        BranchManager => 4,
        Chef => 3,
        Courier => 2,
        Customer => 1,
        _ => 0
    };

    private static int GetMaxRank(IEnumerable<string> userRoles)
    {
        return userRoles
            .Select(GetRank)
            .DefaultIfEmpty(0)
            .Max();
    }

    public static bool IsHigher(IEnumerable<string> callerRoles, IEnumerable<string> targetRoles)
    {
        var callerRank = GetMaxRank(callerRoles);
        var targetRank = GetMaxRank(targetRoles);

        return callerRank > targetRank;
    }

    public static IEnumerable<string> GetFilteredRoles(IEnumerable<string> unFilteredRoles)
    {
        var filteredRoles = unFilteredRoles.Where(role => GetRank(role) != 0);
        return filteredRoles;
    }
}