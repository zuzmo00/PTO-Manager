using System.Security.Claims;

namespace SzabadsagKezeloWebApp.Services;

public interface IAktualisFelhasznaloService
{
    string Torzs { get; }
    string? Nev { get; }
    string? Szerep { get; }
    bool IsAuthenticated { get; }
}

public class AktualisFelhasznaloService : IAktualisFelhasznaloService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AktualisFelhasznaloService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Torzs =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new Exception("Torzs nem található");

    public string? Nev =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public string? Szerep =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}