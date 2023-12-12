namespace CMS.Api.Models
{
    public class UserResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public UserData? userData { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
    public class UserData
    {
        public string? Id { get; set; }
    }
}
