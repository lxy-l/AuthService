using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Core.IdentityService;
using Core.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{ 
    /// <summary>
    /// 授权
    /// </summary>
    [Authorize]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        public IJwtService jwtService { get; }
        public AuthController(IJwtService jwt)
        {
            jwtService = jwt;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Task<JwtTokenViewModel> JwtToken()
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,"Name"),
                    new Claim(JwtRegisteredClaimNames.Jti, "Id"),
                    new Claim(ClaimTypes.SerialNumber, "Id")
            };
            //throw new Exception("d");
            return Task.FromResult(jwtService.GetJwtToken(claims));
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RefreshToken()
        {
            return null;
        }
    }
}
