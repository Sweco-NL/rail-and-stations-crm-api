using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Data.Authorization;

public class RoleHandler(SwecoUserRepository swecoUserRepository, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<RoleRequirement>
{
    private readonly SwecoUserRepository _swecoUserRepository = swecoUserRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        if (!context?.User?.Identity?.IsAuthenticated ?? false)
        {
            return Task.CompletedTask;
        }

        string? swecoUserId = context?.User?.Identity?.Name;

        if (swecoUserId == null)
        {
            return Task.CompletedTask;
        }

        var swecoUser = _swecoUserRepository.GetBySwecoId(swecoUserId, s => s.SwecoUserRoles);

        // Store the authenticated user in HttpContext for later retrieval
        if (swecoUser != null && _httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.HttpContext.Items["CurrentUser"] = swecoUser;
        }

        // TODO: model the user not existing as a different requirement?
        if (swecoUser == null)
        {
            return Task.CompletedTask;
        }

        if (swecoUser.Active && swecoUser.SwecoUserRoles.Any(r => r.Role == requirement.Role))
        {
            context?.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
