using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Interfaces
{
    public interface ITokenService
    {
        SecurityToken GetToken(List<Claim> claims);
        TokenValidationParameters GetTokenValidationParameters();
        string WriteToken(SecurityToken token);
    }
}