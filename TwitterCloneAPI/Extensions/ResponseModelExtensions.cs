using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace TwitterCloneAPI.Extensions;

public static class ResponseModelExtensions
{
    public static IActionResult ToActionResult<TResponse>(this ResponseResult<TResponse> response)
    {
        if (response.IsSuccess)
            return new OkObjectResult(response.Data);

        if (response.StatusCode == StatusCodes.Status401Unauthorized)
            return new UnauthorizedObjectResult(response.GetErrorModel());
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return new NotFoundObjectResult(response.GetErrorModel());
        if (response.StatusCode == StatusCodes.Status500InternalServerError)
            return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = StatusCodes.Status500InternalServerError };
        return new BadRequestObjectResult(response.GetErrorModel());
    }

    public static IActionResult ToActionResult(this ResponseResult response)
    {
        if (response.IsSuccess)
            return new OkResult();

        if (response.StatusCode == StatusCodes.Status401Unauthorized)
            return new UnauthorizedObjectResult(response.GetErrorModel());
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return new NotFoundObjectResult(response.GetErrorModel());
        if (response.StatusCode == StatusCodes.Status500InternalServerError)
            return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = StatusCodes.Status500InternalServerError };
        return new BadRequestObjectResult(response.GetErrorModel());
    }

    private static ResponseErrorModel GetErrorModel(this ResponseResult response)
    {
        return new ResponseErrorModel()
        {
            ErrorCode = response.ErrorCode,
            Errors = response.Errors,
            ErrorMessage = response.ErrorMessage,
            StatusCode = response.StatusCode
        };
    }
}