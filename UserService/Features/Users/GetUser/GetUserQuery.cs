using Keycloak.Net.Models.Users;
using MediatR;

namespace UserService.Features.Users.GetUser;

public record GetUserQuery(string Id, string UserId) : IRequest<User?>;