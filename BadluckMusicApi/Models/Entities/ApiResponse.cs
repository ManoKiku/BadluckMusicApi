namespace BadluckMusicApi.Models.Entities
{
    public class ApiResponse
    {
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public object? Data { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
