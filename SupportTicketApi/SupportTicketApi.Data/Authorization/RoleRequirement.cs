using Microsoft.AspNetCore.Authorization;
using SupportTicketApi.Core.Models.Enums;

namespace SupportTicketApi.Data.Authorization;

public class RoleRequirement(Role role): IAuthorizationRequirement
{
    public readonly Role Role = role;
}
