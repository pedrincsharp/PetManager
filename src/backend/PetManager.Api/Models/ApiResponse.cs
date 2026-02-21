namespace PetManager.Api.Models;

public class ApiResponse<T>
{
    public string Status { get; set; } = string.Empty;
    public string Msg { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Success(string status, string msg, T? data)
        => new ApiResponse<T> { Status = status, Msg = msg, Data = data };

    public static ApiResponse<T> Error(string status, string msg, T? data = default)
        => new ApiResponse<T> { Status = status, Msg = msg, Data = data };
}
