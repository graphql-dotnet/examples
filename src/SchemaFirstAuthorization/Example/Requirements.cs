using System.Threading.Tasks;
using GraphQL.Authorization;
using Microsoft.AspNetCore.Http;

namespace Example
{
    /// <summary>
    /// This represents a custom Authorization Requirement. These can be used
    /// to get data from an external source (such as a database) or check
    /// information that isn't directly available in the ClaimsPrincipal.
    ///
    /// For example, you can use IHttpContextAccessor to check data on the HttpContext.
    /// </summary>
    public class UserIsJoeRequirement : IAuthorizationRequirement
    {
        private readonly IHttpContextAccessor _accessor;

        public UserIsJoeRequirement(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task Authorize(AuthorizationContext context)
        {
            // this is the same as context.User
            if (_accessor.HttpContext.User.Identity?.Name != "Joe")
            {
                context.ReportError("User is not Joe!");
            }
            return Task.CompletedTask;
        }
    }
}
