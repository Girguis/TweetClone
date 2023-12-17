namespace Core;
public struct Result<TData, TException>
{
    public TData Data { get; }
    public TException Exception { get; }
    public bool IsSuccess { get; }

    internal Result(TData data)
    {
        Data = data;
        Exception = default(TException);
        IsSuccess = true;
    }

    internal Result(TException exception)
    {
        Data = default(TData);
        Exception = exception;
        IsSuccess = false;
    }
}

public struct Result<TException>
{
    public TException Exception { get; }
    public bool IsSuccess { get; }

    public Result()
    {
        Exception = default(TException);
        IsSuccess = true;
    }

    public Result(TException exception)
    {
        Exception = exception;
        IsSuccess = false;
    }
}