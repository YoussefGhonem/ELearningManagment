namespace CoursesManagment.DTO.Security.User.Configuration;

public class JwtConfig
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryDuration { get; set; } = 1440;
    public int MaxFailedAccessAttempts { get; set; } = 5;
}