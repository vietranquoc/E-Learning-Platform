using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ELearningPlatform_Authentication.ActionFilters;
using ELearningPlatform_Authentication.TokenValidators;

namespace ELearningPlatform_Authentication.Handlers
{
    public interface IProfileService
    {
        #region Methods
        string GenerateJwt(Claim[] claims, JwtModel jwtConfiguration);

        T DecodeJwt<T>(string token);

        void ByPassAuthorizationFilter(AuthorizationHandlerContext authorizationHandlerContext, 
                                        IAuthorizationRequirement requirement,
                                        bool bAnonymousAccessAttributeCheck = false);
        #endregion
    }

    public class ProfileService : IProfileService
    {
        #region Methods
        public void ByPassAuthorizationFilter(AuthorizationHandlerContext authorizationHandlerContext, 
                                                IAuthorizationRequirement requirement, 
                                                bool bAnonymousAccessAttributeCheck)
        {
            // Anonymous access attribute must be checked 
            if (bAnonymousAccessAttributeCheck)
            {
                // Cast AuthorizationHandlerContext to AuthorizationFilterContext
                var authorizationFilterContext = (AuthorizationFilterContext)authorizationHandlerContext.Resource;

                // No allow anonymous attribute has been found
                if (!authorizationFilterContext.Filters.Any(x => x is ByPassAuthorizationAttribute))
                    return;
            }

            // User does not have primary identity
            if (authorizationHandlerContext.User.Identities.All(x => x.Name != "Anonymous"))
                authorizationHandlerContext.User.AddIdentity(new GenericIdentity("Anonymous"));
            authorizationHandlerContext.Succeed(requirement);

        }

        public T DecodeJwt<T>(string token)
        {
            return default(T);
        }

        public string GenerateJwt(Claim[] claims, JwtModel jwtConfiguration)
        {
            var systemTime = DateTime.Now;
            var expiration = systemTime.AddSeconds(jwtConfiguration.LifeTime);

            // Create the JWT and write it to a string 
            var jwt = new JwtSecurityToken(jwtConfiguration.Issuer,
                                        jwtConfiguration.Audience,
                                        claims,
                                        systemTime,
                                        expiration,
                                        jwtConfiguration.SigningCredentials);

            // From specific information, write token
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        #endregion
    }
}
