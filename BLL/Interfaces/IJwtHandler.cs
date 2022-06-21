using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes operations needed for creating of the JWT token.
    /// </summary>
    public interface IJwtHandler
    {
        /// <summary>
        /// Returns SigningCredentials object with configured and encoded secret information.
        /// </summary>
        /// <returns>SigningCredentials object with secret key.</returns>
        SigningCredentials GetSigningCredentials();

        /// <summary>
        /// Generates JwtSecurityToken based on the SigningCredentials object
        /// and on the claims.
        /// </summary>
        /// <param name="signingCredentials">Object with secret key.</param>
        /// <param name="claims">Collection of claims.</param>
        /// <returns>Configured JwtSecurityToken object.</returns>
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims);
    }
}
