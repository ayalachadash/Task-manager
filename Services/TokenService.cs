using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Interfaces;

namespace TaskManager.Services
{
    public class TokenService: ITokenService
    {
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        private static string issuer = "https://TaskManager.com";
        public SecurityToken GetToken(List<Claim> claims) =>
         new JwtSecurityToken(
             issuer,
             issuer,
             claims,
             expires: DateTime.Now.AddDays(10.0),
             signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
         );
        public TokenValidationParameters GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };
        public string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);
    }
}