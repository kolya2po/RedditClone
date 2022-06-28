using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business
{
    /// <summary>
    /// Implements IJwtHandler interface.
    /// </summary>
    public class JwtHandler : IJwtHandler
    {
        private readonly IConfigurationSection _jwtSettings;

        /// <summary>
        /// Initializes new instance of the JwtHandler class.
        /// </summary>
        /// <param name="configuration">Object with application's configuration.</param>
        public JwtHandler(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("Auth");
        }

        /// <inheritdoc />
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("secret").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <inheritdoc />
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);
        }
    }
}
