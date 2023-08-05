using System.Security.Claims;

namespace ServiceStationAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        Guid? GetUserId { get; } 
    }
    public class UserContextService:IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public Guid? GetUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    }
}
