namespace Application.Response;

public class ResponseModel<TData> : ResponseErrorModel
{
    public TData Data { get; set; }
    public bool IsSuccess
    {
        get
        {
            return string.IsNullOrEmpty(ErrorCode) &&
                   string.IsNullOrEmpty(ErrorMessage) &&
                   (Errors == null || Errors.Count == 0);
        }
    }
}
