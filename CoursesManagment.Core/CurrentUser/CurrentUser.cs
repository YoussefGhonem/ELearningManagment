using CoursesManagment.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CoursesManagment.Core.CurrentUser;

public static class CurrentUser
{
    private static IHttpContextAccessor? _httpContextAccessor;

    #region Logged In User Claims

    public static string BaseUrl => GetBaseUrl();
    public static Guid? Id => string.IsNullOrEmpty(GetClaimValue(ClaimKeys.Id)) ? null : Guid.Parse(GetClaimValue(ClaimKeys.Id)!);
    public static string Name => GetClaimValue(ClaimKeys.Name);
    public static string Email => GetClaimValue(ClaimKeys.Email);
    public static string ImageUrl => GetClaimValue(ClaimKeys.ImageUrl);
    public static List<ApplicationRolesEnum>? Roles => GetRoles();
    public static string? Token => GetAuthorizationToken();

    #endregion

    #region Helper Methods

    private static string GetClaimValue(string key)
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated) return string.Empty;

        var value = user?.Claims?.FirstOrDefault(x => x.Type == key)?.Value;
        return value ?? string.Empty;

    }

    private static bool? GetBoolOrNull(string value)
    {
        if (string.IsNullOrEmpty(value)) return null;
        return bool.Parse(value);
    }

    private static List<ApplicationRolesEnum>? GetRoles()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated) return null;

        var roles = user?.Claims?
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => Enum.Parse<ApplicationRolesEnum>(x.Value))
            .ToList();
        return roles;
    }

    private static string GetBaseUrl()
    {
        var request = _httpContextAccessor?.HttpContext?.Request;
        // return $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
        return $"https://{request?.Host}{request?.PathBase}";
    }

    private static string? GetAuthorizationToken()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];
        return token;
    }

    #endregion

    internal static void InitializeHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}