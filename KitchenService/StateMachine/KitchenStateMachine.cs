using KitchenService.Models.Enities.Enums;

namespace KitchenService.StateMachine;

public static class KitchenStateMachine
{
    private static readonly IReadOnlyDictionary<KitchenStatus, HashSet<KitchenStatus>> AllowedTransitions =
        new Dictionary<KitchenStatus, HashSet<KitchenStatus>>
        {
            [KitchenStatus.Queued] =
            [
                KitchenStatus.Accepted,
                KitchenStatus.Rejected,
                KitchenStatus.Cancelled
            ],

            [KitchenStatus.Accepted] =
            [
                KitchenStatus.Preparing,
                KitchenStatus.Cancelled
            ],

            [KitchenStatus.Preparing] =
            [
                KitchenStatus.Ready,
                KitchenStatus.Cancelled
            ]
        };

    public static bool CanTransition(KitchenStatus current, KitchenStatus next)
    {
        return AllowedTransitions.TryGetValue(
                   current, out var allowed)
               && allowed.Contains(next);
    }

    public static bool EnsureCanTransition(KitchenStatus current, KitchenStatus next)
    {
        return CanTransition(current, next);
    }
}