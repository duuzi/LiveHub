using Api.LiveHub.Common.Helper;
using Api.LiveHub.Common.Logger;
using Api.LiveHub.Domain.Models;
using Api.LiveHub.Dto;
using Api.LiveHub.Infrastrue.DataContext;
using Api.LiveHub.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;

namespace Api.LiveHub.Controllers
{
    public class AccountController : BaseController
    {
        private BusinessDbContext _context;
        private IMapper _mapper;

        public AccountController(BusinessDbContext context, IMapper mapper, IDistributedCache distributeCache) : base(
            distributeCache, context, mapper
            )
        {
            _mapper = mapper;
            _context = context;
        }

        [Route("IsExitUserWx"), HttpGet, Authorize]
        public ActionResult IsExitUserWx(string openid)
        {
            bool flag = false;
            var user = _context.Account.Where(_ => _.OpenId == openid);
            if (user == null || user.Count() <= 0)
                flag = false;
            else
                flag = true;
            return Ok(new ResultViewModel
            {
                success = true,
                data = flag,
                msg = "ok"
            });
        }


        /// <summary>
        /// app登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginByApp")]
        [AllowAnonymous]
        public ActionResult LoginByApp([FromForm] AccountDto input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.AccountNo))
                {
                    throw new Exception("请填写账号或邮箱");
                }
                if (string.IsNullOrWhiteSpace(input.Password))
                {
                    throw new Exception("请填写密码");
                }
                string str = MD5Helper.MD5Encrypt32(input.Password);

                var accountViewModel = new AccountViewModel();

                var user = _context.Account.FirstOrDefault(_ => _.AccountNo == input.AccountNo||_.MailAddress==input.MailAddress);
                if (user == null)
                    throw new Exception("账号或邮箱不存在!");
                if (str != user.Password)
                {
                    throw new Exception("密码错误!");
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
                string jwtStr = JwtHelper.IssueJwt(tokenModel, TokenType.AccessToken);//登录，获取到一定规则的 Token 令牌

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
            catch (Exception ex)
            {
                Logger.Error("登录失败:" + ex.Message);
                throw new Exception("登录失败!");
            }
        }
        /// <summary>
        /// app注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Route("Regist"), HttpPost, AllowAnonymous]
        public ActionResult Regist([FromForm]AccountDto input)
        {
            if (string.IsNullOrEmpty(input.MailAddress))
            {
                throw new Exception("请填写邮箱!");
            }
            if (string.IsNullOrEmpty(input.AccountNo))
            {
                throw new Exception("请填写账号!");
            }
            if (string.IsNullOrEmpty(input.Password))
            {
                throw new Exception("请填写密码!");
            }
            string str = MD5Helper.MD5Encrypt32(input.Password);
            var model = new Account
            {
                AccountNo = input.AccountNo,
                MailAddress = input.MailAddress.Trim(),
                Password = str,
                Status = AccountStatus.Enabled
            };
            var account = _context.Account.Add(model).Entity;
            _context.SaveChanges();
            var role = _context.Role.FirstOrDefault(_ => _.RoleName == "普通用户");
            UserRole userrole = new UserRole
            {
                RoleId = role.Id,
                UserId = account.Id
            };
            _context.UserRole.Add(userrole);
            _context.SaveChanges();
            return Ok(new ResultViewModel
            {
                success = true,
                data = "注册成功",
                msg = "注册成功"
            });
        }

        [Route("GetUser"), HttpGet, Authorize]
        public ActionResult GetUser()
        {
            long uid = GetLoginUser().Id;
            var user = _context.Account.FirstOrDefault(_ => _.Id == uid);
            var vmodel = _mapper.Map<AccountViewModel>(user);
            return Ok(new ResultViewModel
            {
                success = true,
                data = vmodel,
                msg = "获取用户信息成功"
            });
        }
        /// <summary>
        /// 更新手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Route("UpdateUser"), HttpPost, Authorize]
        public ActionResult UpdateUser([FromForm]AccountDto input)
        {
            var usermodel = GetLoginUser();
            var user = _context.Account.FirstOrDefault(_ => _.Id == usermodel.Id);
            AccountViewModel vmodel = null;
            user.PhoneNumber = input.PhoneNumber;
            _context.Account.Update(user);
            vmodel = _mapper.Map<AccountViewModel>(user);
            _context.SaveChanges();
            return Ok(new ResultViewModel
            {
                success = true,
                data = vmodel,
                msg = "更新成功"
            });
        }

        [Route("UpdateOrAddUserWx"), HttpPost,Authorize]
        public ActionResult UpdateOrAddUserWx(AccountDto input)
        {
            if (string.IsNullOrEmpty(input.OpenId))
                throw new Exception("openid不存在");
            var user = _context.Account.FirstOrDefault(_ => _.OpenId == input.OpenId);
            AccountViewModel vmodel = null;
            if (user == null)//新增用户
            {
                var hasUser = _context.Account.FirstOrDefault(_ => _.AccountNo == input.AccountNo);
                if (hasUser != null)
                    throw new Exception("工号已存在!");
                var model = new Account
                {
                    AccountNo = input.AccountNo,
                    AccountName = input.AccountName,
                    OpenId = input.OpenId,
                    NickName = input.NickName,
                    Status = AccountStatus.Enabled,
                };
                _context.Account.Add(model);
                vmodel = _mapper.Map<AccountViewModel>(model);
            }
            else//修改工号等
            {
                var hasUser = _context.Account.FirstOrDefault(_ => _.AccountNo == input.AccountNo && _.Id != user.Id);
                if (hasUser != null)
                    throw new Exception("工号已存在!");
                user.AccountNo = input.AccountNo;
                user.AccountName = input.AccountName;
                _context.Account.Update(user);
                vmodel = _mapper.Map<AccountViewModel>(user);
            }

            _context.SaveChanges();

            return Ok(new ResultViewModel
            {
                success = true,
                data = vmodel,
                msg = "更新成功"
            });
        }

        [Route("CheckHasAccountNoWx"), HttpGet]
        public ActionResult CheckHasAccountNoWx(string openid)
        {
            bool flag = false;
            var user = _context.Account.Where(_ => _.OpenId == openid).FirstOrDefault();
            if (user != null)
                if (!string.IsNullOrWhiteSpace(user.AccountNo))
                    flag = true;
            return Ok(new ResultViewModel
            {
                success = true,
                data = flag,
                msg = "ok"
            });
        }
    }
}