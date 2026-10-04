namespace OrderDispatch.Contracts.Enums;

public enum OrderStatus
{
    Created,
    PendingKitchen,
    KitchenAccepted,
    PaymentPending,
    Paid,
    Preparing,
    Ready,
    CourierAssigned,
    OutForDelivery,
    Delivered,
    Cancelled,
    Rejected
}