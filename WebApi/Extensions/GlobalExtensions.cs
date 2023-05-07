using System.Security.Claims;
using WebApi.Controllers;

namespace WebApi.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            var claims = httpContext.User.Claims.ToList();

            // return httpContext.User.Claims.Single(x => x.Type == "id").Value;
            return claims.Find(c => c.Type == "uid")?.Value;
        }

        public static string GetComanyId(this HttpContext context)
            => context is null ? string.Empty :
                context.User?.Claims?.FirstOrDefault(s => s.Type == "companyId")?.Value;

        public static Guid GetLogedUserCompanyId(this BaseApiController controller)
        {
            if (controller.Request.HttpContext.User == null)
            {
                return Guid.Empty;
            }
            var claims = controller.Request.HttpContext.User.Claims.ToList();
            var companyId = claims.Find(c => c.Type == "companyId")?.Value;
            return string.IsNullOrEmpty(companyId) ? Guid.Empty : Guid.Parse(companyId);
        }

        public static List<string> GetUserRoles(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return new List<string>();
            }

            var claims = httpContext.User.Claims.ToList();
            return claims.Where(c => c.Type == ClaimTypes.Role)?.Select(x => x.Value).ToList();
        }

    }
}
