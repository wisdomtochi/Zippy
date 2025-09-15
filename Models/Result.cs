namespace Zippy.Models
{
    public class Result<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public static Result<T> Success(T data, string? message = null)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Message = message ?? string.Empty,
                Data = data
            };
        }
        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Data = null
            };
        }
    }
}
