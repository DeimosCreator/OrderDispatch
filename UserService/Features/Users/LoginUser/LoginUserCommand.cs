using MediatR;

namespace UserService.Features.Users.LoginUser;

public record LoginUserCommand(string Username, string Password) : IRequest<TokenResponse?>;