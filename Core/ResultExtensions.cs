namespace Core;
public static class ResultExtensions<TData, TException>
{
    public static Result<TData, TException> Success(TData data)
    {
        return new Result<TData, TException>(data);
    }

    public static Result<TData, TException> Error(TException exception)
    {
        return new Result<TData, TException>(exception);
    }

    public static Result<TException> Success()
    {
        return new Result<TException>();
    }
}