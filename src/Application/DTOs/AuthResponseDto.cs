namespace Application.DTOs
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
