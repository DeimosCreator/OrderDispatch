using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;
using UserService.Options;

namespace UserService.Features.Users.LoginUser;

public class LoginUserCommandHandler(
    IHttpClientFactory httpClientFactory,
    IOptions<KeycloakSettings> keycloakOptions,
    IConfiguration configuration) : IRequestHandler<LoginUserCommand, TokenResponse?>
{
    public async Task<TokenResponse?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();
        var url = $"{keycloakOptions.Value.Url}/realms/{keycloakOptions.Value.Realm}/protocol/openid-connect/token";
        
        var bodyParams = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", configuration["KEYCLOAK_LOGIN_CLIENT_ID"] ?? "api-gateway" },
            { "username", request.Username },
            { "password", request.Password }
        };

        var content = new FormUrlEncodedContent(bodyParams);
        
        var response = await client.PostAsync(url, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null; 
        }

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TokenResponse>(responseJson);
    }
}