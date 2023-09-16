namespace DocHub.Sample.Common;
public enum ResultCode
{
    Default = 0,
    NeedOtpConfirmation = 100,
    Unauthorized = 401
}

public class Result
{
    public bool Success { get; set; }
    public ResultCode Code { get; set; }
    public List<string> Messages { get; set; } = new();

    public Result()
    {
    }
    public Result(bool success)
    {
        Success = success;
    }
    public Result(params string[] messages)
    {
        Messages = messages.ToList();
    }
}

public class Result<TResultData> : Result
{
    public TResultData? Data { get; set; }

    public Result()
    {
    }

    public Result(bool success)
    {
        Success = success;
    }

    public Result(TResultData data)
    {
        Success = true;
        Data = data;
    }

    public Result(params string[] messages)
    {
        Messages = messages.ToList();
    }
}
