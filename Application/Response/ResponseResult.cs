using System.Net;
using System.Xml.Linq;

namespace Application.Response;

public class ResponseResult : ResponseErrorModel
{
    public bool IsSuccess
    {
        get
        {
            return string.IsNullOrEmpty(ErrorCode) &&
                   string.IsNullOrEmpty(ErrorMessage) &&
                   (Errors == null || Errors.Count == 0);
        }
    }

    public static ResponseResult CreateSuccess()
        => new();

    public static ResponseResult CreateError(string errorCode)
        => new()
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ErrorCode = errorCode
        };

    public static ResponseResult CreateError(HttpStatusCode statusCode,
                                             string errorCode)
        => new()
        {
            StatusCode = (int)statusCode,
            ErrorCode = errorCode
        };

    public static ResponseResult CreateError(HttpStatusCode statusCode, string errorCode, List<string> errors)

    => new()
    {
        StatusCode = (int)statusCode,
        ErrorCode = errorCode,
        Errors = errors
    };
}


public sealed class ResponseResult<TData> : ResponseResult
{
    public TData Data { get; set; }

    public static ResponseResult<TData> CreateSuccess(TData data)
        => new()
        { Data = data };

    public static new ResponseResult<TData> CreateError(string errorCode)

        => new()
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ErrorCode = errorCode
        };

    public static new ResponseResult<TData> CreateError(HttpStatusCode statusCode, string errorCode)

        => new()
        {
            StatusCode = (int)statusCode,
            ErrorCode = errorCode
        };

    public static new ResponseResult<TData> CreateError(HttpStatusCode statusCode, string errorCode , List<string> errors)

        => new()
        {
            StatusCode = (int)statusCode,
            ErrorCode = errorCode,
            Errors = errors
        };
}
