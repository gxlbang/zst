using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp
{
    public class UserLoginFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //处理用户登录状态
            HttpCookie Cookie = filterContext.RequestContext.HttpContext.Request.Cookies["WebUserInfo"];
            WebData webBLL = new WebData();
            if (!webBLL.ValidateLoginWebUser(Cookie))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<script type='text/javascript'>alert('未登录或登录超时,请重新登录!');location.href='/Ui/Index';</script>",
                };
            }
        }
    }
}