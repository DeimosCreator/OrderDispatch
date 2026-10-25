using MediatR;
using UserService.Models.Enums;

namespace UserService.Features.Users.AssignRoleUser;

public record AssignRoleUserCommand(string UserId, string AssignRoleUserId, string Role) : IRequest<AssignRoleUserResult>;