using OrderDispatch.Contracts.Enums;
using OrderService.Models.Enities.Enums;

namespace OrderService.StateMachine;

public static class OrderStateMachine
{
    private static readonly IReadOnlyDictionary<OrderStatus, HashSet<OrderStatus>> AllowedTransitions =
        new Dictionary<OrderStatus, HashSet<OrderStatus>>
        {
            [OrderStatus.Created] =
            [
                OrderStatus.PendingKitchen,
                OrderStatus.Cancelled
            ],

            [OrderStatus.PendingKitchen] =
            [
                OrderStatus.KitchenAccepted,
                OrderStatus.Rejected,
                OrderStatus.Cancelled
            ],

            [OrderStatus.KitchenAccepted] =
            [
                OrderStatus.PaymentPending,
                OrderStatus.Cancelled
            ],

            [OrderStatus.PaymentPending] =
            [
                OrderStatus.Paid,
                OrderStatus.Cancelled
            ],

            [OrderStatus.Paid] =
            [
                OrderStatus.Preparing,
                OrderStatus.Cancelled
            ],

            [OrderStatus.Preparing] =
            [
                OrderStatus.Ready
            ],

            [OrderStatus.Ready] =
            [
                OrderStatus.CourierAssigned
            ],

            [OrderStatus.CourierAssigned] =
            [
                OrderStatus.OutForDelivery
            ],

            [OrderStatus.OutForDelivery] =
            [
                OrderStatus.Delivered
            ]
        };

    public static bool CanTransition(OrderStatus current, OrderStatus next)
    {
        return AllowedTransitions.TryGetValue(
                   current, out var allowed)
               && allowed.Contains(next);
    }

    public static bool EnsureCanTransition(OrderStatus current, OrderStatus next)
    {
        return CanTransition(current, next);
    }
}