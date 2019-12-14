using Api.LiveHub.Common.Helper;
using Api.LiveHub.Infrastrue.DataContext;
using Api.LiveHub.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Senparc.Weixin;
using System.Linq;

namespace Api.LiveHub.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    //[EnableCors("LiveHub")]
    public class BaseController : ControllerBase
    {
        protected string AppId
        {
            get
            {
                return Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
            }
        }
        private IMapper _mapper;
        private AccountViewModel LoginUser = null;
        private BusinessDbContext _context;
        protected readonly IDistributedCache _distributeCache;

        protected BaseController(IDistributedCache distributeCache, BusinessDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _distributeCache = distributeCache;
        }

        protected AccountViewModel GetLoginUser()
        {
            if (LoginUser == null)
            {
                var jwtStr = Request.Headers["Authorization"].ToString();
                jwtStr = jwtStr.Substring(7);
                var loginModel = JwtHelper.SerializeJwt(jwtStr);

                // 1. 先从缓存中获取数据
                var accountViewModelStr = _distributeCache.Get("AppAccount" + loginModel.Uid);
                if(accountViewModelStr!=null)
                    LoginUser = SerializeHelper.ByteArrayToObject<AccountViewModel>(accountViewModelStr);

                //string account = HttpContext.Session.GetString("Account");
                //byte[] accountBytes = HttpContext.Session.Get("Account");

                // 2. 缓存失效时，从数据库查询数据
                //if (accountBytes == null || accountBytes.Length == 0)
                //if (string.IsNullOrEmpty(account))
                if (LoginUser == null)
                {
                    var user  = _context.Account.FirstOrDefault(_=>_.Id.ToString() == loginModel.Uid);
                    LoginUser = _mapper.Map<AccountViewModel>(user);
                }
                else
                {
                    var user = _context.Account.FirstOrDefault(_ => _.Id.ToString() == loginModel.Uid);
                    LoginUser = _mapper.Map<AccountViewModel>(user);
                }

                //else
                //{
                //LoginUser = JsonHelper.ParseFormByJson<AccountViewModel>(account);
                //LoginUser = SerializeHelper.ByteArrayToObject<AccountViewModel>(accountBytes);
                //}
            }
            return LoginUser;
        }
    }
}