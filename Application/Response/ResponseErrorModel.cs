namespace Application.Response;

public class ResponseErrorModel
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public List<string> Errors { get; set; }
    public int StatusCode { get; set; }
}