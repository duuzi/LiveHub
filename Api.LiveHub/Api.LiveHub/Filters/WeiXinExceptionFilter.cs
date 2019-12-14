using Api.LiveHub.Common.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.LiveHub.Filters
{
    public class WeiXinExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = string.Format("消息类型：{0}\t\n消息内容：{1}\t\n引发异常的方法：{2}\t\n引发异常源：{3}"
                , context.Exception.GetType().Name
                , context.Exception.Message
                 , context.Exception.TargetSite
                 , context.Exception.Source + context.Exception.StackTrace
                 );
            // 打印错误日志
            Logger.Error(message,context.Exception);

            //context.Result = new JsonResult(new
            //{
            //    success = false,
            //    msg = context.Exception.Message,
            //    code = (int)HttpStatusCode.InternalServerError
            //});
            //context.ExceptionHandled = true;
            //context.HttpContext.Response.Clear();
            // 处理ajax request
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Because its a exception raised after ajax invocation, Lets return Json
                context.Result = new JsonResult(new 
                {
                    success = false,
                    msg = context.Exception.Message,
                    code = (int)HttpStatusCode.InternalServerError
                });
                context.ExceptionHandled = true;
                context.HttpContext.Response.Clear();
            }
        }
    }
}
