namespace BaridikExpress.Application.DTO.AuthModules
{
    public class RefreshTokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiresAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}