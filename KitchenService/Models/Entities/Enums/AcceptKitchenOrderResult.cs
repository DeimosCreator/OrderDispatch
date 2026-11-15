namespace KitchenService.Models.Entities.Enums;

public enum AcceptKitchenOrderResult
{
    KitchenOrderNotFound,
    KitchenNotFound,
    KitchenNotActive,
    KitchenBusy,
    CannotAccept,
    Accepted
}