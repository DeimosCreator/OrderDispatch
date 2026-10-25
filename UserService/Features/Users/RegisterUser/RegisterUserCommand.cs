using Keycloak.Net.Models.Users;
using MediatR;

namespace UserService.Features.Users.RegisterUser;

public record RegisterUserCommand(string UserName, string Password, string Email, string FirstName, string LastName)
    : IRequest<User?>;