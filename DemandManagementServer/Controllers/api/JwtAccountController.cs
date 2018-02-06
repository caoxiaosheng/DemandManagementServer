using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DemandManagementServer.Controllers.API
{
    [Produces("application/json")]
    public class JwtAccountController : Controller
    {
        private readonly IUserService _userService;

        public JwtAccountController(IUserService userService)
        {
            _userService = userService;
        }
        
        [Route("api/login")]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var user = _userService.CheckUser(loginViewModel.UserName, loginViewModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("miaomiaoo(=•ェ•=)m");
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"all"),
                    new Claim(JwtClaimTypes.Issuer,"zyc"),
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName)
                }),
                Expires = expiresAt,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
            });
        }
    }
}
