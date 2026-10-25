namespace UserService.Options;

public class KeycloakSettings
{
    public string Realm { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string AdminClientId { get; set; } = string.Empty;
    public string AdminClientSecret { get; set; } = string.Empty;
    public string LoginClientId { get; set; } = string.Empty;
}