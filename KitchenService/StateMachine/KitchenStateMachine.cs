using KitchenService.Models.Entities.Enums;
using OrderDispatch.Contracts.Enums;

namespace KitchenService.StateMachine;

public readonly record struct KitchenStatusInput(KitchenStatus? Status)
{
    public static implicit operator KitchenStatusInput(KitchenStatus status) => new(status);
    public static implicit operator KitchenStatusInput(OrderStatus status) => new(status.ToKitchenStatus());
}

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
                KitchenStatus.Paid,
                KitchenStatus.Cancelled
            ],
            
            [KitchenStatus.Paid] = 
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
    
    public static KitchenStatus? ToKitchenStatus(this OrderStatus orderStatus) => orderStatus switch
    {
        OrderStatus.PendingKitchen => KitchenStatus.Queued,
        OrderStatus.KitchenAccepted => KitchenStatus.Accepted,
        OrderStatus.Paid => KitchenStatus.Paid,
        OrderStatus.Preparing => KitchenStatus.Preparing,
        OrderStatus.Ready => KitchenStatus.Ready,
        OrderStatus.Cancelled => KitchenStatus.Cancelled,
        OrderStatus.Rejected => KitchenStatus.Rejected,
        _ => null
    };

    public static bool CanTransition(KitchenStatus current, KitchenStatus next)
    {
        return AllowedTransitions.TryGetValue(
                   current, out var allowed)
               && allowed.Contains(next);
    }

    public static bool EnsureCanTransition(KitchenStatus current, KitchenStatusInput next, out KitchenStatus resolvedStatus)
    {
        if (next.Status is { } target && CanTransition(current, target))
        {
            resolvedStatus = target;
            return true;
        }

        resolvedStatus = current;
        return false;
    }
}