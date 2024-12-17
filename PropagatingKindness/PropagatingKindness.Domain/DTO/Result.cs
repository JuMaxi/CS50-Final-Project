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
