namespace Taxually.TechnicalTest.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ApiResponse SuccessResponse(string message = "Success") => new ApiResponse { Success = true, Message = message };
        public static ApiResponse ErrorResponse(string message) => new ApiResponse { Success = false, Message = message };
    }
}
