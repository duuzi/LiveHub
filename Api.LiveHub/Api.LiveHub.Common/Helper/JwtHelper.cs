using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.LiveHub.Common.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// 
        /// 1. 用户登录成功的时候，一次性给他两个Token，分别为AccessToken和RefreshToken，AccessToken用于正常请求，RefreshToken作为刷新AccessToken的凭证。
        /// 2. AccessToken的有效期较短，例如一小时，短一点安全一些。RefreshToken有效期可以设置长一些，例如一天、一周等。
        /// 3. 当AccessToken即将过期的时候，例如提前5分钟，客户端利用RefreshToken请求指定的API获取新的AccessToken并更新本地存储中的AccessToken。
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel, TokenType tokenType)
        {
            string iss = Appsettings.app(new string[] { "JWT", "Issuer" });
            string secret = Appsettings.app(new string[] { "JWT", "Key" });
            string aud = Appsettings.app(new string[] { "JWT", "Audience" });

            // RefreshAudience，与原Audience值不一致，
            // 作用是使RefreshToken不能用于访问应用服务的业务API，而AccessToken不能用于刷新Token。
            if (tokenType == TokenType.RefreshToken)
            {
                aud += "Refresh";
            }
            // 过期时间，目前AccessToken是5分钟过期，RefreshToken是12小时过期
            var ExpirationTime = DateTime.Now.AddMinutes(5);
            if (tokenType == TokenType.RefreshToken)
            {
                ExpirationTime = DateTime.Now.AddHours(12);
            }

            // var claims = new Claim[] //old
            var claims = new List<Claim>
            {
                /*
                 * 特别重要：
                 *   1、这里将用户的部分信息，比如 uid 存到了Claim 中，如果你想知道如何在其他地方将这个 uid从 Token 中取出来，
                 *      请看下边的SerializeJwt() 方法，或者在整个解决方案，搜索这个方法，看哪里使用了！
                 *   2、你也可以研究下 HttpContext.User.Claims，具体的你可以看看 Policys/PermissionHandler.cs 类中是如何使用的。
                 */
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                // 这个就是过期时间，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(ExpirationTime).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss, iss),
                new Claim(JwtRegisteredClaimNames.Aud, aud),   
                // 这个Role是官方UseAuthentication要验证的Role，我们就不用手动设置Role这个属性了
                new Claim(ClaimTypes.Role, tokenModel.Role), // 为了解决一个用户多个角色（比如：Admin,System），用下边的方法
            };

            // 可以将一个用户的多个角色全部赋予；
            // 作者：DX 提供技术支持；
            // claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            // 秘钥（SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常）
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModelJwt
            {
                Uid = jwtToken.Id,
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
    }

    public enum TokenType
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
