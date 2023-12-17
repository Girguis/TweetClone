using Application.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace TwitterCloneAPI.Filters;

public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        context.HttpContext.Items.TryGetValue("Guid", out object userGuid);
        if (userGuid != null && !string.IsNullOrEmpty(userGuid?.ToString()))
            return;

        context.Result = new UnauthorizedObjectResult(new ResponseErrorModel()
        {
            ErrorCode = "unauthorized",
            StatusCode = (int)HttpStatusCode.Unauthorized
        });

    }
}