using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectClosure.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Error(string msg)
        {
            ViewBag.msg = msg;

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}