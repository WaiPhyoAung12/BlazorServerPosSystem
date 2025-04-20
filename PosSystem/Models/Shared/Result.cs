namespace PosSystem.Models.Shared;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsError => !IsSuccess;
    //public EnumErrorType? ErrorType { get; set; }
    public string? Message { get; set; }
    public List<string> MessageList { get; set; } = new();
    public T? Data { get; set; }
    public string? Code { get; set; }

    public static Result<T> Success(string? message, T? data = default, string code = null)
    {
        return new Result<T> { IsSuccess = true, Message = message, Data = data, Code = code };
    }

    public static Result<T> Fail(
        string? message, string code = null)
    {
        return new Result<T> { IsSuccess = false, Message = message, Code = code };
    }

    public static Result<T> FailValidation(List<string>? messageList)
    {
        return new Result<T> { IsSuccess = false, MessageList = messageList};
    }

    //public static Result<T> Fail(Exception ex, T data = default)
    //{
    //    return new Result<T> { IsSuccess = false, Message = ex.ToString(), Data = data };
    //}
}
