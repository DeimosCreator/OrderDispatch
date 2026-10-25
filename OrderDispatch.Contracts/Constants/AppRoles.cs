namespace OrderDispatch.Contracts.Constants;

public static class AppRoles
{
    public const string SuperAdmin = "super-admin";
    public const string CompanyOwner = "company-owner";
    public const string BranchManager = "branch-manager";
    public const string Chef = "chef";
    public const string Courier = "courier";
    public const string Customer = "customer";

    public static int GetRank(string role) => role switch
    {
        SuperAdmin => 6,
        CompanyOwner => 5,
        BranchManager => 4,
        Chef => 3,
        Courier => 2,
        Customer => 1,
        _ => 0
    };

    public static bool IsHigher(string currentRole, string targetRole)
    {
        var currentRank = GetRank(currentRole);
        var targetRank = GetRank(targetRole);

        return currentRank > targetRank;
    }
}