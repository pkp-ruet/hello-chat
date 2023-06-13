namespace HelloChat.Models;

public class AuthenticationConfiguration
{
    public string AccessTokenSecret { get; set; }
    public string RefreshTokenSecret { get; set; }
    public double AccessTokenExpirationTime { get; set; }
    public int RefreshTokenExpirationTime { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
