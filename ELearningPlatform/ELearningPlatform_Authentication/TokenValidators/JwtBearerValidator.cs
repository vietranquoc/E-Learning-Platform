using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Authentication.TokenValidators
{
    public class JwtBearerValidator : ISecurityTokenValidator
    {
        #region Properties
        /// <summary>
        /// Determines whether this validator can validate tokens.
        /// Always returns true, meaning this class is capable of validating tokens.
        /// </summary>
        public bool CanValidateToken => true;

        /// <summary>
        /// Specifies the maximum size of the token in bytes.
        /// This property allows setting a limit on the token size that this validator can handle.
        /// </summary>
        public int MaximumTokenSizeInBytes { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Checks whether this validator can read the token.
        /// Always returns true, meaning it can read any token passed to it.
        /// </summary>
        /// <param name="securityToken">The JWT token to check.</param>
        /// <returns>Returns true because this validator can read any token.</returns>
        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        /// <summary>
        /// Validates the JWT token based on the provided validation parameters.
        /// </summary>
        /// <param name="securityToken">The JWT token to validate.</param>
        /// <param name="validationParameters">The parameters used to validate the token.</param>
        /// <param name="validatedToken">The token that is successfully validated.</param>
        /// <returns>Returns a ClaimsPrincipal containing claims if the token is valid.</returns>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            // Handler which is for handling security token
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            // Validate jwt
            // Use the handler to validate the JWT token with the specified validation parameters
            var claimsPrincipal = jwtSecurityTokenHandler
                                .ValidateToken(securityToken, validationParameters, out validatedToken);

            return claimsPrincipal;
        }
        #endregion
    }
}
