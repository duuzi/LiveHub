using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.LiveHub.Common.Helper;
using Api.LiveHub.Common.Logger;
using Api.LiveHub.Domain.Models;
using Api.LiveHub.Dto;
using Api.LiveHub.Infrastrue.DataContext;
using Api.LiveHub.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.LiveHub.Controllers
{
    public class BusinessController : BaseController
    {
        private BusinessDbContext _context;
        private IMapper _mapper;
        //private IHostingEnvironment _hostingEnvironment;
        public BusinessController(BusinessDbContext context,IMapper mapper, IDistributedCache distributeCache):base(
            distributeCache,context,mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        //获取待办事项列表
        [Route("GetToDoList")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetToDoList() {
            var user = GetLoginUser();
            var list = _context.ToDoList.Where(_ => _.AccountId == user.Id).OrderByDescending(_=>_.Id);
            var todolist =  _mapper.Map<List<ToDoViewModel>>(await list.ToListAsync());
            //var returnlist = new ToDolistViewModel();
            //returnlist.doList = todolist.Where(_ => _.ToDoType == ToDoType.ToDo).ToList();
            //returnlist.thingsList = todolist.Where(_ => _.ToDoType == ToDoType.Things).ToList();
            //returnlist.trainingList = todolist.Where(_ => _.ToDoType == ToDoType.Training).ToList();
            return Ok(new
            {
                success = true,
                msg = "OK",
                data = todolist
            });
        }
        [Route("AddToDo")]
        [HttpPost]
        [Authorize]
        public ActionResult AddToDo([FromForm]ToDoDto input)
        {
            var user = GetLoginUser();
            var model = new ToDoList
            {
                AccountId = user.Id,
                Status = ToDoStatus.未完成,
                ToDoType = input.ToDoType,
                Name = input.ToDoName,
                Checked = false,
                
            };
            _context.ToDoList.Add(model);
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "操作成功!",
            });
        }
        [Route("UpdToDo")]
        [HttpPost]
        [Authorize]
        public ActionResult UpdToDo([FromForm]ToDoDto input)
        {
            var model = _context.ToDoList.Where(_ => _.Id == input.Id).FirstOrDefault();
            if (model == null)
                throw new Exception("任务不存在!");
            model.Name = input.ToDoName;
            model.Checked = input.Checked;
            model.Status = input.Status;
            model.ToDoType = input.ToDoType;
            _context.ToDoList.Update(model);
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "操作成功!",
            });
        }
        [Route("DelToDo")]
        [HttpPost]
        [Authorize]
        public ActionResult DelToDo([FromForm]string ids)
        {
            var user = GetLoginUser();
            if(string.IsNullOrWhiteSpace(ids))
            {
                throw new Exception("没有要删除的选项!");
            }
            var idList = ids.Split(',').ToList();
            idList.ForEach(_ =>
            {
                var entity = _context.ToDoList.Find(long.Parse(_));
                _context.ToDoList.Remove(entity);
            });
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "操作成功!",
            });
        }
        [Route("UploadScore")]
        [HttpPost]
        [Authorize]
        public ActionResult UploadScore([FromForm]int score)
        {
            var user = GetLoginUser();
            var model = _context.GameScore.Where(_ => _.AccountId==user.Id).FirstOrDefault();
            if (model == null)
            {
                var item = new GameScore
                {
                    AccountId = user.Id,
                    Score=score,
                    CreateTime=DateTime.Now
                };
                _context.GameScore.Add(item);
            }
            else
            {
                if (score > model.Score)
                {
                    model.Score = score;
                    _context.GameScore.Update(model);
                }
            }
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "分数上传成功!",
            });
        }
        [Route("GetGameRank")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetGameRank()
        {
            var list = await _context.GameScore.OrderByDescending(_ => _.Score).Take(5).ToListAsync();
            return Ok(new
            {
                success = true,
                msg = "OK",
                data = list
            });
        }
        [Route("AddSignIn")]
        [HttpPost]
        [Authorize]
        public ActionResult AddSignIn([FromForm]string signInText)
        {
            var user = GetLoginUser();
            //string openid = GetLoginUser().OpenId;
            var list = _context.SignIn.Where(_ => _.AccountId == user.Id && _.SignDate.Date == DateTime.Today);
            if (list.Any())
            {
                throw new Exception("请勿重复签到");
            }
            var model = new SignIn
            {
                AccountId = user.Id,
                SignText = signInText,
                SignDate = DateTime.Now
            };
            _context.SignIn.Add(model);
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "签到成功!",
            });
        }
        [Route("CheckSignIn")]
        [HttpGet]
        [Authorize]
        public ActionResult CheckSignIn()
        {
            var flag = false;
            var user = GetLoginUser();
            //string openid = GetLoginUser().OpenId;
            var model = _context.SignIn.Where(_ => _.AccountId == user.Id && _.SignDate.Date == DateTime.Today);
            if(model.Any())
            {
                flag = true;
            }
            var text = model.FirstOrDefault()?.SignText;
            return Ok(new
            {
                data = new { 
                    signed = flag,
                    signtext = text
                },
                success = true,
                msg = "获取成功!",
            });
        }
        [Route("GetSignInList")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSignInList()
        {
            //string openid = GetLoginUser().OpenId;
            var user = GetLoginUser();
            var list = _context.SignIn.Where(_ => _.AccountId==user.Id);
            list = list.Where(_ => _.SignDate >= DateTime.Today.AddDays(-15));
            var returnList = _mapper.Map<List<SignInViewModel>>(await list.ToListAsync());
            return Ok(new
            {
                success = true,
                msg = "OK",
                data = returnList
            });
        }
        [Route("GetBusinessList")]
        [HttpGet]
        [Authorize]
        public ActionResult GetBusinessList([FromQuery]bool isSeven)
        {
            //string openid = GetLoginUser().OpenId;
            var user = GetLoginUser();
            var list = _context.Business.Where(_=>_.AccountId==user.Id);
            if(isSeven)
                list = list.Where(_ => _.SignDate >= DateTime.Today.AddDays(-7));
            var returnList = _mapper.Map<List<BusinessViewModel>>(list.ToList());
            Logger.Info("测试log");
            return Ok(new
            {
                success = true,
                msg = "OK",
                data = returnList
            });
        }

        [Route("SignIn")]
        [HttpPost]
        [Authorize]
        public ActionResult SignIn(BusinessDto input)
        {
            //string openid = GetLoginUser().OpenId;
            //if (string.IsNullOrEmpty(openid))
            //    throw new Exception("openid不存在");
            //var user = _context.Account.FirstOrDefault(_ => _.OpenId == openid);
            var user = GetLoginUser();
            if (user==null || string.IsNullOrWhiteSpace(user.AccountNo))
                throw new Exception("请先完善工号等信息再签到");
            var model = new Business
            {
                AccountId = user.Id,
                From = input.From,
                To = input.To,
                CreateDate = DateTime.Now,
                AMorPM = input.AMorPM,
                BusinessType = input.BusinessType,
                SignDate = DateTime.Now
            };
            _context.Business.Add(model);
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "签到成功!",
            });
        }

        [Authorize(Roles ="助理")]
        [HttpGet]
        [Route("DownloadExcel")]
        public IActionResult DownloadExcel([FromQuery]ExcelInputDto input)
        {
            //var filter = Request.Query["DownloadDateType"];
            var type = input.DownloadDateType;
            var query = _context.Business.AsTracking();   
            if (input.DownloadDateType.HasValue)
            {
                switch (type)
                {
                    case DownloadDate.Yesterday:
                        query = query.Where(_ => _.SignDate >= DateTime.Today.AddDays(-1)&& _.SignDate<DateTime.Today);
                        break;
                    case DownloadDate.Today:
                        query = query.Where(_ => _.SignDate >= DateTime.Today && _.SignDate < DateTime.Today.AddDays(1));
                        break;
                    case DownloadDate.LastWeek:
                        query = query.Where(_ => _.SignDate >= DateTime.Today.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek) - 7) && _.SignDate < DateTime.Today.AddDays(6 - Convert.ToInt16(DateTime.Now.DayOfWeek) - 7).AddDays(1));
                        break;
                    case DownloadDate.ThisWeek:
                        query = query.Where(_ => _.SignDate >= DateTime.Today.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek)) && _.SignDate < DateTime.Today.AddDays(6 - Convert.ToInt16(DateTime.Now.DayOfWeek)).AddDays(1));
                        break;
                    case DownloadDate.LastMonth:
                        query = query.Where(_ => _.SignDate >= DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(-1) && _.SignDate < DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddDays(-1).AddDays(1));
                        break;
                    case DownloadDate.ThisMonth:
                        query = query.Where(_ => _.SignDate >= DateTime.Today.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek)) && _.SignDate < DateTime.Today.AddDays(6 - Convert.ToInt16(DateTime.Now.DayOfWeek)).AddDays(1));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (input.StartDate.HasValue)
                    query = query.Where(_ => _.SignDate > input.StartDate.Value.Date);
                if (input.EndDate.HasValue)
                    query = query.Where(_ => _.SignDate < input.EndDate.Value.Date.AddDays(1));
            }
            var excelList = _mapper.Map<List<BusinessExcelViewModel>>(query.ToList());
            string[] title = {
                "工号",
                "姓名",
                "打卡类型",
                "出发地",
                "目的地",
                "签到日期",
                "时间段",
                "备注"
            };

            var buffer = ExcelHelper.OutputExcel(excelList, title);
            return File(buffer.ToArray(), "application/ms-excel", $"{DateTime.Now.ToString("yyMMddHHmm")}.xlsx");
            //if (string.IsNullOrEmpty(fileName))
            //{
            //    return NotFound();
            //}
            //string _folder = $@"{_hostingEnvironment.ContentRootPath}\Upload";
            //string fileExt = Path.GetExtension(fileName);
            ////获取文件的ContentType
            //var provider = new FileExtensionContentTypeProvider();
            //var memi = provider.Mappings[fileExt];

            //string controller = RouteData.Values["controller"].ToString();
            //var path = _folder + $@"\{fileName}";
            //var stream = System.IO.File.OpenRead(path);
            //string encodeFilename = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.GetEncoding("UTF-8"));
            
            //return File(stream, "application/octet-stream", fileName);

        }
    }
}