using ProjectClosure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectClosure.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult Index(int CompId, int ProjectId)
        {
            string error = "";
            try
            {
                BL_Registry.SetLog("Entered Home/Index2");
                BL_Registry.SetLog("CompId = " + CompId.ToString());
                string status = "";
                string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getPrjStatus,@p1="+ ProjectId);
                BL_Registry.SetLog("Home/Index qry = " + retrievequery);
                DataSet ds = DBClass.GetData(retrievequery, CompId, ref error);
                status = ds.Tables[0].Rows[0][0].ToString();
                BL_Registry.SetLog("Home/Index status = " + status);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BL_Registry.SetLog(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Error(string msg)
        {
            ProjectClosureModel _data = (ProjectClosureModel)Session["PCData"];
            int CompanyId = Convert.ToInt32(_data.SearchCriteria.Cid);
            TempData["CompanyId"] = CompanyId;
            ViewBag.msg = msg;
            return View();
        }

        public ActionResult Success()
        {
            ProjectClosureModel _data = (ProjectClosureModel)Session["PCData"];
            int CompanyId = Convert.ToInt32(_data.SearchCriteria.Cid);
            TempData["CompanyId"] = CompanyId;
            return View();
        }
    }
}