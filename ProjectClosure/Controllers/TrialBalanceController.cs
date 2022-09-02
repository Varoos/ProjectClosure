using ClosedXML.Excel;
using Newtonsoft.Json;
using ProjectClosure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectClosure.Controllers
{
    public class TrialBalanceController : Controller
    {
        string errors1 = "";
        public ActionResult TBIndex(int CompanyId)
        {
            var _projects = GetProjects(CompanyId);
            ViewBag.Project = _projects;
            var _division = GetDivision(CompanyId);
            ViewBag.Division = _division;
            var _costcenter = GetCostCenter(CompanyId);
            ViewBag.CostCenter = _costcenter;
            var _site = GetSites(CompanyId);
            ViewBag.Site = _site;
            var _activity = GetActivity(CompanyId);
            ViewBag.Activity = _activity;
            var _servicecode = GetServiceCode(CompanyId);
            ViewBag.ServiceCode = _servicecode;
            var _AcGrp = getAccountGrpslist(CompanyId);
            ViewBag.AcGrp = _AcGrp;
            var _Ac = getAccountlist(CompanyId);
            ViewBag.Ac = _Ac;
            ViewBag.CompId = CompanyId;
            return View();
        }
        public IEnumerable<SelectListItem> GetProjects(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getAllProjects");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetDivision(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getDivision");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetCostCenter(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getCostCenter");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetSites(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getSite");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetActivity(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getActivity");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetServiceCode(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getServiceCode");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> getAccountGrpslist(int cid)
        {
            string retrievequery = string.Format($@"exec pCore_CommonSp @Operation='getAccountGroup'");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public IEnumerable<SelectListItem> getAccountlist(int cid)
        {
            string retrievequery = string.Format($@"exec pCore_CommonSp @Operation='getAccounts'");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "(" + ds.Tables[0].Rows[i]["sCode"].ToString() + ") " + ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        //public ActionResult TrialBalanceReport(int CompanyId, string Projects, string Divisions, string CostCenters, string Sites, string Activities, string ServiceCodes,string FromDate, string ToDate,string DivisionNames,string CostCenterNames,string AcIds,string AcNames, string AcGrpIds, string AcGrpNames)
        [HttpPost]
        public ActionResult TrialBalanceReport2(Search _search)
        {
            try
            {
                #region QueryData
                string Strsql = string.Format($@"exec pCore_CommonSp @Operation = TrailBalanceData,@p0 = '{_search.Project}',@p6 = '{_search.CostCenter}',@p7='{_search.Site}',@p8='{_search.ServiceCode}',@p9 = '{_search.Activity}',@p5 = '{_search.Division}',@p3='{_search.FromDate}',@p11='{_search.ToDate}',@p12='{_search.AcIds}',@p13='{_search.AcGrpIds}'");
                DataSet ds = DBClass.GetDataSet(Strsql, _search.Cid, ref errors1);

                int table = ds.Tables.Count;
                TrialBalanceModel reportObj = new TrialBalanceModel();
                List<Transactions> listobj = new List<Transactions>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        listobj.Add(new Transactions
                        {
                            CostCenter = dr["CostCenter"].ToString(),
                            CostCenterCode = dr["CCcode"].ToString(),
                            Site = dr["Site"].ToString(),
                            SiteCode = dr["SiteCode"].ToString(),
                            AccountType = dr["AccountType"].ToString(),
                            AccountGroup = dr["AccountGroup"].ToString(),
                            AccountCode = dr["AccountCode"].ToString(),
                            AccountName = dr["AccountName"].ToString(),
                            Project = dr["Project"].ToString(),
                            Activity = dr["Activity"].ToString(),
                            ServiceCode = dr["ServiceCode"].ToString(),
                            Division = dr["Division"].ToString(),
                            Currency = dr["CurrencyName"].ToString(),
                            CurrencyCode = dr["Currency"].ToString(),
                            TranDr = Convert.ToDecimal(dr["TranDr"].ToString()),
                            TranCr = Convert.ToDecimal(dr["TranCr"].ToString()),
                            TranBal = Convert.ToDecimal(dr["TranBal"].ToString()),
                            Debit = Convert.ToDecimal(dr["Debit"].ToString()),
                            Credit = Convert.ToDecimal(dr["Credit"].ToString()),
                            Bal = Convert.ToDecimal(dr["Bal"].ToString()),
                        });
                    }
                    reportObj.listTransactions = listobj;
                    reportObj.Search = _search;
                    Session["reportObj"] = reportObj;
                    return Json("Success", JsonRequestBehavior.AllowGet);
                    #endregion
                }
                else
                {
                    return Json("No Data", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                DBClass.SetLog("Getting TB Report View. EXCEPTION = " + ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpGet]
        public ActionResult TrialBalanceReport()
        {
            TrialBalanceModel reportObj = (TrialBalanceModel)Session["reportObj"];
            return View(reportObj);
        }
        [HttpPost]
        public FileResult ExcelGenerate()
        {
            TrialBalanceModel reportObj = (TrialBalanceModel)Session["reportObj"];
            #region TempData
            int CompanyId = Convert.ToInt32(reportObj.Search.Cid);
            string DivisionNames= reportObj.Search.DivisionNames.ToString();
            string CostCenterNames = reportObj.Search.CostCenterNames.ToString();
            string FromDate = reportObj.Search.FromDate.ToString();
            string ToDate= reportObj.Search.ToDate.ToString();
            List<Transactions> trans = new List<Transactions>();
            trans = (List<Transactions>)reportObj.listTransactions;
            #endregion

            System.Data.DataTable data = new System.Data.DataTable("TRIAL BALANCE");
            #region DataColumns
            data.Columns.Add("Account Type", typeof(string));
            data.Columns.Add("Account Group", typeof(string));
            data.Columns.Add("Account", typeof(string));
            data.Columns.Add("Account Description", typeof(string));
            data.Columns.Add("Cost Center Name", typeof(string));
            data.Columns.Add("Cost Center Code", typeof(string));
            data.Columns.Add("Site Name", typeof(string));
            data.Columns.Add("Site Code", typeof(string));
            data.Columns.Add("Division", typeof(string));
            data.Columns.Add("Project", typeof(string));
            data.Columns.Add("Activities", typeof(string));
            data.Columns.Add("Service Code", typeof(string));
            data.Columns.Add("Currency Name", typeof(string));
            data.Columns.Add("Currency Code", typeof(string));
            data.Columns.Add("Trans Dr", typeof(decimal));
            data.Columns.Add("Trans Cr", typeof(decimal));
            data.Columns.Add("Trans Bal", typeof(decimal));
            data.Columns.Add("Base Dr", typeof(decimal)); 
            data.Columns.Add("Base Cr", typeof(decimal));
            data.Columns.Add("Base Bal", typeof(decimal));

            #endregion


            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("TRIAL BALANCE");
                var dataTable = data;
                int colcount = 21;


                var wsReportNameHeaderRange = ws.Range(ws.Cell(1, 2), ws.Cell(1, colcount));//ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dataTable.Columns.Count + 1)));
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportNameHeaderRange.Style.Fill.BackgroundColor = XLColor.Yellow;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "TRIAL BALANCE";


                var wsReportNameHeaderRange1 = ws.Range(ws.Cell(2, 2), ws.Cell(2, colcount));//ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dataTable.Columns.Count + 1)));
                wsReportNameHeaderRange1.Style.Font.Bold = true;
                wsReportNameHeaderRange1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportNameHeaderRange1.Style.Fill.BackgroundColor = XLColor.Yellow;
                wsReportNameHeaderRange1.Merge();
                wsReportNameHeaderRange1.Value = "(Division = "+DivisionNames+"       CostCenters = "+CostCenterNames+")";

                var wsReportNameHeaderRange2 = ws.Range(ws.Cell(3, 2), ws.Cell(3, colcount));//ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dataTable.Columns.Count + 1)));
                wsReportNameHeaderRange2.Style.Font.Bold = true;
                wsReportNameHeaderRange2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportNameHeaderRange2.Style.Fill.BackgroundColor = XLColor.Yellow;
                wsReportNameHeaderRange2.Merge();
                wsReportNameHeaderRange2.Value = "From  " + FromDate + " To " + ToDate ;

                int r = 5;
                int cell = 2;
                for (int i = 1; i < data.Columns.Count; i++)
                {
                    cell = 2;
                    #region Headers
                    ws.Cell(r, cell++).Value = "Account Type";
                    ws.Cell(r, cell++).Value = "Account Group";
                    ws.Cell(r, cell++).Value = "Account";
                    ws.Cell(r, cell++).Value = "Account Description";
                    ws.Cell(r, cell++).Value = "Cost Center Name";
                    ws.Cell(r, cell++).Value = "Cost Center Code";
                    ws.Cell(r, cell++).Value = "Site Name";
                    ws.Cell(r, cell++).Value = "Site Code";
                    ws.Cell(r, cell++).Value = "Division";
                    ws.Cell(r, cell++).Value = "Project";
                    ws.Cell(r, cell++).Value = "Activities";
                    ws.Cell(r, cell++).Value = "Service Code";
                    ws.Cell(r, cell++).Value = "Currency Name";
                    ws.Cell(r, cell++).Value = "Currency Code";
                    ws.Cell(r, cell++).Value = "Trans Dr";
                    ws.Cell(r, cell++).Value = "Trans Cr";
                    ws.Cell(r, cell++).Value = "Trans Bal";
                    ws.Cell(r, cell++).Value = "Base Dr";
                    ws.Cell(r, cell++).Value = "Base Cr";
                    ws.Cell(r, cell++).Value = "Base Bal";

                    #endregion
                }
                var TableRange = ws.Range(ws.Cell(r, 2), ws.Cell(r, colcount));
                TableRange.Style.Font.FontColor = XLColor.White;
                TableRange.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 115, 170);
                TableRange.Style.Font.Bold = true;
                TableRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                
                int c = 2;

                #region TableLoop
                decimal dr = 0;
                decimal cr = 0;
                decimal bal = 0;
                int count = 1;
                foreach (var obj in trans)
                {
                    dr = Convert.ToDecimal(obj.Debit) + dr;
                    bal = Convert.ToDecimal(obj.Bal) + bal;
                    cr = Convert.ToDecimal(obj.Credit) + cr;
                    //count++;
                    c = 2;
                    r++;
                    ws.Range(ws.Cell(r, c), ws.Cell(r, colcount)).Style.Fill.BackgroundColor = XLColor.FromHtml("#90EE90");
                    ws.Cell(r, c++).Value = obj.AccountType;
                    ws.Cell(r, c++).Value = obj.AccountGroup;
                    ws.Cell(r, c++).Value = obj.AccountCode;
                    ws.Cell(r, c++).Value =obj.AccountName;
                    ws.Cell(r, c++).Value = obj.CostCenter;
                    ws.Cell(r, c++).Value = obj.CostCenterCode;
                    ws.Cell(r, c++).Value = obj.Site;
                    ws.Cell(r, c++).Value = obj.SiteCode;
                    ws.Cell(r, c++).Value = obj.Division;
                    ws.Cell(r, c++).Value = obj.Project;
                    ws.Cell(r, c++).Value = obj.Activity;
                    ws.Cell(r, c++).Value = obj.ServiceCode;
                    ws.Cell(r, c++).Value = obj.Currency;
                    ws.Cell(r, c++).Value = obj.CurrencyCode;
                    ws.Cell(r, c++).Value = obj.TranDr.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.TranCr.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.TranBal.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.Debit.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.Credit.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.Bal.ToString("N", new CultureInfo("en-US"));
                }


                //Grand Total Row
                r++;
                c = 2;
                ws.Range(ws.Cell(r, c++), ws.Cell(r, 18)).Merge().Value = "Total";
                ws.Cell(r, 19).Value = dr.ToString("N", new CultureInfo("en-US"));
                ws.Cell(r, 20).Value = cr.ToString("N", new CultureInfo("en-US"));
                ws.Cell(r, colcount).Value = bal.ToString("N", new CultureInfo("en-US"));

                ws.Range("B" + r + ":Z" + r + "").Style.Font.Bold = true;
                r++;


                #endregion

                TableRange = ws.Range(ws.Cell(4, 2), ws.Cell(r - 1, colcount));
                TableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                TableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                TableRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range(ws.Cell(4, colcount-6), ws.Cell(r, colcount)).Style.NumberFormat.Format = "0.00";

                //ws.Columns("A:BZ").AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TrialBalance" + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx");
                }
            }
        }
        public ActionResult _reportpage(string s,JqueryDatatableParam param)
        {
            TrialBalanceModel reportObj = (TrialBalanceModel)Session["reportObj"];
            List<Transactions> _data = reportObj.listTransactions;
            Session["BodyData"] = _data;
            //DBClass.SetLog("listdata ListDetail= " + _data.ListDetail.Count);
            //DBClass.SetLog("listdata ListSummary= " + _data.ListSummary.Count);
            var displayResult = _data.Skip(param.iDisplayStart)
             .Take(param.iDisplayLength).ToList();
            _data = displayResult;
            Session.Remove("BodyData");
            List<Transactions> _data2 = reportObj.listTransactions;
            Session["BodyData"] = _data2;
            return Json(new
            {
                param.sEcho,
                iTotalRecords = _data.Count,
                iTotalDisplayRecords = _data2.Count,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);
        }
    }
}