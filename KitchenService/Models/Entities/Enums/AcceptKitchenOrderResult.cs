namespace KitchenService.Models.Entities.Enums;

public enum AcceptKitchenOrderResult
{
    KitchenOrderNotFound,
    KitchenNotFound,
    KitchenBusy,
    CannotAccept,
    Accepted
}