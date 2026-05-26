using Microsoft.AspNetCore.Mvc;
using SupportTicketApi.Core.Models;

namespace SupportTicketApi.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected SwecoUser? CurrentUser
    {
        get
        {
            if (HttpContext.Items.TryGetValue("CurrentUser", out var userObj))
            {
                return userObj as SwecoUser;
            }

            return null;
        }
    }
}