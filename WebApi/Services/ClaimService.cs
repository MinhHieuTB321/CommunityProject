using System.Security.Claims;
using Application.Services.Interfaces;

namespace WebApi.Services;

public class ClaimsService : IClaimsService
{
    public ClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue("ID");
        GetCurrentUser = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
    }
    public Guid GetCurrentUser { get; }
}