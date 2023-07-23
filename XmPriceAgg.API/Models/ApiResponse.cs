namespace XmPriceAgg.API.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public Error? Error { get; set; }
    public object? Data { get; set; }

    public static ApiResponse EnsureSuccess(object result) => new()
    {
        Success = true,
        Data = result
    };

    public static ApiResponse EnsureError(string msg, string code) => new()
    {
        Success = true,
        Error = new Error
        {
            ErrorCode = code,
            Message = msg
        }
    };
}