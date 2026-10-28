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
    
    private static string GetRole(int role) => role switch
    {
         6 => SuperAdmin,
         5 => CompanyOwner,
         4 => BranchManager,
         3 => Chef,
         2 => Courier,
         1 => Customer,
         _ => ""
    };

    public static int GetMaxRank(IEnumerable<string> userRoles) => userRoles.Select(GetRank).DefaultIfEmpty(0).Max();

    public static bool IsHigher(IEnumerable<string> callerRoles, IEnumerable<string> targetRoles) =>
        GetMaxRank(callerRoles) > GetMaxRank(targetRoles);

    public static IEnumerable<string> GetFilteredRoles(IEnumerable<string> unFilteredRoles) =>
        unFilteredRoles.Where(role => GetRank(role) != 0);

    public static string GetAllRoles(string role) =>
        string.Join(",", Enumerable.Range(1, GetRank(role)).Select(GetRole).Where(r => !string.IsNullOrEmpty(r)));
}