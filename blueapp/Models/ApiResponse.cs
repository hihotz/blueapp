namespace blackapi.Models
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
