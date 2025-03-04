using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ELearningPlatform_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid GetCurrentUserId(IHttpContextAccessor _httpContext)
        {
            var identity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Guid.Empty;
            }

            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("Id"));

            return Guid.Parse(userClaim?.Value);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetCurrentUserName(IHttpContextAccessor _httpContext)
        {
            var identity = _httpContext.HttpContext?.User.Identity as ClaimsIdentity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("Name"));

            return userClaim?.Value;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetCurrentUser(IHttpContextAccessor _httpContext)
        {
            var identity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("UserId"));

            return userClaim?.Value;
        }
    }
}
