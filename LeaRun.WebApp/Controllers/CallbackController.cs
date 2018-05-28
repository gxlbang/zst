using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class CallbackController : Controller
    {
        public static LogHelper log = LogFactory.GetLogger(typeof(DataAccess.DbHelper));
        //
        // GET: /Callback/

        public ActionResult Index()
        {
            log.Error(Request.QueryString.ToString());
            return View();
        }

    }
}
