using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

namespace Platinum.Common
{
    public static class JWTToken
    {
        public static string GenerateJSONWebToken(string EmailAddress, string UserId, string CompanyId, string RoleId, string SecretKey)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim("EmailAddress",EmailAddress),
                   new Claim("LoggedInUserId", UserId),
                   new Claim("CompanyId", CompanyId),
                   new Claim("RoleId", RoleId)
                }),
                Expires = DateTime.Now.AddMinutes(200000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
