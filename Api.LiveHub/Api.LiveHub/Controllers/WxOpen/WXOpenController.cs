using Api.LiveHub.Common.Helper;
using Api.LiveHub.Domain.Models;
using Api.LiveHub.Dto;
using Api.LiveHub.Infrastrue.DataContext;
using Api.LiveHub.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using System;
using System.Linq;

namespace Api.LiveHub.Controllers.WxOpen
{
    [Route("[controller]")]
    public partial class WxOpenController : ControllerBase
    {
        //public static readonly string Token = Config.SenparcWeixinSetting.WxOpenToken;//与微信小程序后台的Token设置保持一致，区分大小写。
        //public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.WxOpenEncodingAESKey;//与微信小程序后台的EncodingAESKey设置保持一致，区分大小写。
        protected readonly IDistributedCache _distributeCache;
        private readonly IConfiguration _configuration;
        public static readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;//与微信小程序后台的AppId设置保持一致，区分大小写。
        public static readonly string WxOpenAppSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;//与微信小程序账号后台的AppId设置保持一致，区分大小写。
        private BusinessDbContext _context;
        private IMapper _mapper;

        readonly Func<string> _getRandomFileName = () => SystemTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
        
        public WxOpenController(IConfiguration configuration, BusinessDbContext context, IMapper mapper, IDistributedCache distributeCache)
        {
            _mapper = mapper;
            _context = context;
            _distributeCache = distributeCache;
            _configuration = configuration;
        }
        [Route("RequestData")]
        [HttpPost]
        public ActionResult RequestData(string nickName)
        {
            var data = new
            {
                msg = string.Format("服务器时间：{0}，昵称：{1}", SystemTime.Now.LocalDateTime, nickName)
            };
            return Ok(data);
        }





        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginByWeixin")]
        [AllowAnonymous]
        public ActionResult LoginByWeixin(string code, string userInfo)
        {
            try
            {
                var jsonResult = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, code);
                if (jsonResult.errcode == ReturnCode.请求成功)
                {

                    //Session["WxOpenUser"] = jsonResult;//使用Session保存登陆信息（不推荐）
                    //使用SessionContainer管理登录信息（推荐）
                    //var unionId = "";
                    //var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);
                    var accountViewModel = new AccountViewModel();
                    var accountDto = SerializeHelper.DeserializeString<AccountDto>(userInfo);

                    var user = _context.Account.FirstOrDefault(_ => _.OpenId == jsonResult.openid);
                    if (user == null)
                    {
                        user = new Account
                        {
                            OpenId = jsonResult.openid,
                            NickName = accountDto.NickName,
                            AvatarUrl = accountDto.AvatarUrl,
                            Status = AccountStatus.Enabled
                        };
                        var account = _context.Account.Add(user).Entity;
                        _context.SaveChanges();
                        //角色控制
                        Role role = _context.Role.FirstOrDefault(_ => _.RoleName == "普通用户");
                        _context.UserRole.Add(new UserRole
                        {
                            RoleId = role.Id,
                            UserId = account.Id
                        });
                        _context.SaveChanges();
                    }
                    //角色控制
                    var userRole = _context.UserRole.FirstOrDefault(_ => _.UserId == user.Id);
                    Role bindRole = userRole == null ? null : userRole.Role;
                    accountViewModel = _mapper.Map<AccountViewModel>(user);
                    //角色控制
                    accountViewModel.Role = _mapper.Map<RoleViewModel>(bindRole);
                    accountViewModel.RoleName = bindRole == null ? null : bindRole.RoleName;
                    // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                    TokenModelJwt tokenModel = new TokenModelJwt { Uid = user.Id.ToString(), Role = accountViewModel.Role == null ? "" : accountViewModel.Role.RoleName };
                    string jwtStr = JwtHelper.IssueJwt(tokenModel,TokenType.AccessToken);//登录，获取到一定规则的 Token 令牌

                    _distributeCache.Set("AppAccount" + tokenModel.Uid, SerializeHelper.ObjectToByteArray(accountViewModel));

                    //注意：生产环境下SessionKey属于敏感信息，不能进行传输！
                    return Ok(new
                    {
                        success = true,
                        msg = "OK",
                        data = new
                        {
                            token = jwtStr,
                            userInfo = accountViewModel
                        }
                    });
                }
                else
                {
                    return Ok(new { success = false, msg = jsonResult.errmsg });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, msg = ex.Message });
            }
        }

        
    }
}