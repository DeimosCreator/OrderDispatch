namespace OrderDispatch.Contracts.Messaging.Events.Users;

public record UserRoleChangedEvent(string UserId, string Role);