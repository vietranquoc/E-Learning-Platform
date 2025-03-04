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
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement> 
    {
        #region Properties
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public RoleRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            // Convert authorization handler context into authorization filter context.
            var authorizationFilterContext = (AuthorizationFilterContext) context.Resource;

            // Check context solidity
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Find HttpContext
            var httpContxt = _httpContextAccessor.HttpContext;

            // Find Account which has been embededinto HttpContext
            if (!httpContxt.Items.ContainsKey(ClaimTypes.Actor))
            {
                // Controller or method allow by pass information analyze
                if (IsAbleToByPass(authorizationFilterContext))
                {
                    context.Succeed(requirement);
                    
                    return Task.CompletedTask;
                }

                context.Fail();
                return Task.CompletedTask;
            }

            // context.Succeed(requirement);
            return Task.CompletedTask;
        }

        private bool IsAbleToByPass(AuthorizationFilterContext authorizationFilterContext)
        {
            return authorizationFilterContext.Filters.Any(x => x is ByPassAuthorizationAttribute);
        }
        #endregion
    }
}
