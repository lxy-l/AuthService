using Core.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.IdentityService
{
    public interface IJwtService
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        JwtTokenViewModel GetJwtToken(List<Claim> claims);

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetUserId(ClaimsPrincipal user);

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        JwtSecurityToken SerializeJwt(string jwtStr);

    }
}
