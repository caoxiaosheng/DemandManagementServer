using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DemandManagementServer.Models;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DemandManagementServer.Controllers.API
{
    [Produces("application/json")]
    public class JwtAccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IWxUserService _wxUserService;

        public JwtAccountController(IUserService userService,IConfiguration configuration, IWxUserService wxUserService)
        {
            _userService = userService;
            _configuration = configuration;
            _wxUserService = wxUserService;
        }
        
        [Route("api/login")]
        public async Task<IActionResult> Login(string code)
        {
            var appid = _configuration["appid"];
            var secret = _configuration["secret"];
            
            string url =
                string.Format(
                    "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code",
                    appid, secret, code);
            WxUserInfo wxUserInfo;
            using (HttpClient httpClient=new HttpClient())
            {
                var result = await httpClient.GetAsync(url);
                result.EnsureSuccessStatusCode();
                var wxUserInfoString =  await result.Content.ReadAsStringAsync();
                wxUserInfo = JsonConvert.DeserializeObject<WxUserInfo>(wxUserInfoString);
            }
            if (_wxUserService.CheckOpenId(wxUserInfo.openid,out var wxUser)==false)
            {
                return Json(new
                {
                    result = false
                });
            }

            var tokenString = CreateJwtToken(wxUser.User.Id.ToString(), wxUser.User.UserName, wxUserInfo.openid);

            return Json(new
            {
                result=true,
                access_token = tokenString,
                userName= wxUser.User.UserName
            });
        }
        
        [Route("api/bind")]
        public async Task<IActionResult> BindUser(string code,string userName,string password)
        {
            var appid = _configuration["appid"];
            var secret = _configuration["secret"];

            string url =
                string.Format(
                    "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code",
                    appid, secret, code);
            WxUserInfo wxUserInfo;
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(url);
                result.EnsureSuccessStatusCode();
                var wxUserInfoString = await result.Content.ReadAsStringAsync();
                wxUserInfo = JsonConvert.DeserializeObject<WxUserInfo>(wxUserInfoString);
            }
            if (wxUserInfo == null)
            {
                return Json(new
                {
                    result = false,
                    reason = "认证信息错误"
                });
            }
            var user = _userService.CheckUser(userName, password);
            if (user == null)
            {
                return Json(new
                {
                    result = false,
                    reason = "用户名或密码错误"
                });
            }

            if (_wxUserService.CheckUserId(user.Id)==false)
            {
                return Json(new
                {
                    result = false,
                    reason = "用户已被绑定"
                });
            }

            _wxUserService.AddWxUser(new WxUser(){OpenId = wxUserInfo.openid, UserId = user.Id});
            var tokenString = CreateJwtToken(user.Id.ToString(), user.UserName, wxUserInfo.openid);
            return Json(new
            {
                result = true,
                reason = "",
                access_token = tokenString,
                userName = user.UserName
            });
        }

        private string CreateJwtToken(string userId,string userName,string openId)
        {
            var jwtkey = _configuration["jwtkey"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtkey);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"all"),
                    new Claim(JwtClaimTypes.Issuer,"zyc"),
                    new Claim(JwtClaimTypes.Id, userId),
                    new Claim(JwtClaimTypes.Name, userName)
                }),
                Expires = expiresAt,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }

    class WxUserInfo
    {
        public string session_key { get; set; }

        public string expires_in { get; set; }

        public string openid { get; set; }
    }
}
