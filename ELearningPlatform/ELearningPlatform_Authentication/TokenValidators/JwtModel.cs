using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Authentication.TokenValidators
{
    public class JwtModel
    {
        #region Properties
        /// <summary>
        /// Issuer of Jwt, indicates who issued the JWT (typically your application).
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The sub (sbject) claim identifies the principal that is the subject
        /// It can be used to identify the user or entity to which the JWT belongs.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The audience for the JWT, indicating who the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// LifeTime of the JWT token in minutes, representing the token's expiration duration.
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// The secret key used to sign the JWT, ensuring the integrity of the token.
        /// </summary>
        public string SecurityKey { get; set; }

        private SigningCredentials _signingCredentials;
        private SymmetricSecurityKey _symmetricSecurityKey;

        /// <summary>
        /// SigningKey is a symmetric security key derived from the SecurityKey string.
        /// It's used to sign the JWT.
        /// </summary>
        public SymmetricSecurityKey SigningKey
        {
            get
            {
                if (_symmetricSecurityKey == null)
                {
                    _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));
                }

                return _symmetricSecurityKey;
            }
        }

        /// <summary>
        /// SigningCredentials contains the signing key and algorithm used to sign the JWT.
        /// </summary>
        public SigningCredentials SigningCredentials
        {
            get
            {
                if (_signingCredentials == null)
                {
                    _signingCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
                }

                return _signingCredentials;
            }
        }
        #endregion
    }
}
