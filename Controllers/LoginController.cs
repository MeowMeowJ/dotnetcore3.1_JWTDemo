using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Controllers
{
    [Route("{controller}")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        public string GetToken()
        {
            // 3 + 2
            SecurityToken securityToken = new JwtSecurityToken(
                // 搭建人
                issuer: "issuer",
                // 订阅人
                audience: "audience",
                // 验证令牌
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("laukitlaukitlaukit")),
                    SecurityAlgorithms.HmacSha256),

                // 过期日期
                expires: DateTime.Now.AddHours(1),
                // 声明
                claims: new Claim[] {}
            );
       

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
