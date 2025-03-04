using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ELearningPlatform_Authentication.ActionFilters;
using ELearningPlatform_Authentication.Requirements;

namespace ELearningPlatform_Authentication.Handlers
{
    public class SolidAccountRepuirementHandler : AuthorizationHandler<SolidAccountRequirement>
    {
        #region Properties
        private readonly IProfileService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public SolidAccountRepuirementHandler(IProfileService identityService, IHttpContextAccessor httpContextAccessor) 
        { 
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SolidAccountRequirement requirement)
        {
            // Convert authorization handler context into authorization filter context
            var authorizationFilterContext = (AuthorizationFilterContext) context.Resource;

            var httpContext = _httpContextAccessor.HttpContext;

            // Find claim identity attached to principal
            var claimIdentity = (ClaimsIdentity) httpContext.User.Identity;

            // Find Id from claims list
            var id = claimIdentity.Claims
                .Where(x => x.Type.Equals("Id"))
                .Select(x => x.Value)
                .FirstOrDefault();  

            if (string.IsNullOrEmpty(id))
            {
                if (authorizationFilterContext == null)
                {
                    context.Fail();
                    return;
                }

                // Method or controller authorization can be by pass
                if (authorizationFilterContext.Filters.Any(x => x is ByPassAuthorizationAttribute))
                {
                    _identityService.ByPassAuthorizationFilter(context, requirement);
                    return;
                }

                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        #endregion
    }
}
