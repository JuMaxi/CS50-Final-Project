namespace PropagatingKindness.Domain.DTO;

public class Result
{
    public bool Success { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;

    public Result(bool success, string errorMessage)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }

    public static Result OK => new Result(true, string.Empty);
    public static Result Fail(string msg) => new Result(false, msg);
}

public class LoginResult : Result
{
    public UserDTO? User { get; private set; }

    public LoginResult(string errorMessage) : base(false, errorMessage)
    {
    }

    public LoginResult(UserDTO userDTO) : base(true, string.Empty)
    {
        User = userDTO;
    }
}

public class Result<T> : Result
{
    public T Content { get; set; }
    public Result(bool success, string errorMessage) : base(success, errorMessage)
    {
    }

    public Result(T content) : base(true, string.Empty)
    {
        Content = content;
    }
}
