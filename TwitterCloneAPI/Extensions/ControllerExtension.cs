using Microsoft.AspNetCore.Mvc;

namespace TwitterCloneAPI.Extensions
{
    public static class ControllerExtension
    {
        public static string? Guid(this ControllerBase controller)
        {
            return controller.HttpContext.Items["Guid"]?.ToString();
        }
    }
}
