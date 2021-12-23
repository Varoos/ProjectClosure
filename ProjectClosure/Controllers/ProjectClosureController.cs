using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectClosure.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ProjectClosure.Controllers
{
    public class ProjectClosureController : Controller
    {
        string errors1 = "";
        public ActionResult Index(int CompanyId)
        {
            ViewBag.CompId = CompanyId;
            //var _projects = GetProjects(CompanyId);
            //ViewBag.Projects = _projects;
            //var _accounts = GetWIPAc(CompanyId);
            //ViewBag.Accounts = _accounts;
            return View();
        }
        public IEnumerable<SelectListItem> GetProjects(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getProjects");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = "("+ ds.Tables[0].Rows[i]["sCode"].ToString()+") "+ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public ActionResult getProjectslist(int cid,string searchtext,string Operation)
        {
            string retrievequery = string.Format($@"exec pCore_CommonSp @Operation='{Operation}', @p4='{searchtext}'");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);
            string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(JSONString, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAccountslist(int cid, string searchtext, string Operation)
        {
            string retrievequery = string.Format($@"exec pCore_CommonSp @Operation='{Operation}', @p4='{searchtext}'");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);
            string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(JSONString, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetWIPAc(int cid)
        {
            string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getAccounts");
            List<SelectListItem> containers = new List<SelectListItem>();
            DataSet ds = DBClass.GetData(retrievequery, cid, ref errors1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                containers.Add(new SelectListItem()
                {
                    Text = ds.Tables[0].Rows[i]["sName"].ToString(),
                    Value = ds.Tables[0].Rows[i]["iMasterId"].ToString(),
                });
            }

            return new SelectList(containers.ToArray(), "Value", "Text");
        }
        public ActionResult getProjectDetails(int CompanyId, int SelectValue)
        {
            string retrievequery = string.Format($@"exec pCore_CommonSp @Operation=getProjectsDetails, @p1={SelectValue}");
            DataSet ds = DBClass.GetData(retrievequery, CompanyId, ref errors1);
            string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(JSONString, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProjectClosureReport(int CompanyId, int Project, string ReportDate, int Account,string ProjectName,string AccountName, string Customer, string Division,int DivisionId,int MLId,int DivisionParentId,int PL_ac,int CustomerId)
        {
            TempData["CompanyId"] = CompanyId;
            TempData["Project"] = Project;
            TempData["ReportDate"] = ReportDate;
            TempData["Account"] = Account;
            DateTime reportDt = Convert.ToDateTime(ReportDate);
           
            #region QueryData


            string Strsql = string.Format($@"exec pCore_CommonSp @Operation = getData,@p1 = {Account},@p2 = {Project}");
            DataSet ds = DBClass.GetDataSet(Strsql, CompanyId, ref errors1);

            int table = ds.Tables.Count;
            ProjectClosureModel reportObj = new ProjectClosureModel();
            List<WIP_Transactions> listobj = new List<WIP_Transactions>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    listobj.Add(new WIP_Transactions
                    {
                        CostCenter = dr["costcenter"].ToString(),
                        Site = dr["site"].ToString(),
                        DocNo = dr["DocNo"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"].ToString()),
                        Dr = Convert.ToDecimal(dr["Debit"].ToString()),
                        Cr = Convert.ToDecimal(dr["Credit"].ToString()),
                        Balance = Convert.ToDecimal(dr["Balance"].ToString()),
                        count= Convert.ToInt32(dr["c"].ToString()),
                        rno = Convert.ToInt32(dr["row_num"].ToString()),
                        DivisionId = DivisionId,
                        CostCenterID = Convert.ToInt32(dr["CostCenterId"].ToString()),
                        SiteId = Convert.ToInt32(dr["SiteId"].ToString()),
                        ProjectId = Project,
                        ML_id = MLId,
                        DivisionParentId = DivisionParentId,
                        PL_ac = PL_ac,
                        CustomerId = CustomerId
                    }) ;
                }
                reportObj.listWIP_Transactions = listobj;
                TempData["listdata"] = listobj;
                TempData.Keep();
                SearchCriteria objA = new SearchCriteria();
                objA.Cid = CompanyId;
                objA.ClosureDate = ReportDate;
                objA.Project = ProjectName;
                objA.Division = Division;
                objA.Customer = Customer;
                objA.WIPAc = AccountName;
                reportObj.SearchCriteria = objA;
            }
            #endregion
            return View(reportObj);
        }

        public ActionResult ProjectClosurePosting(int CompanyId)
        {
            TempData["CompanyId"] = CompanyId;
            TempData.Keep();
            string Message = "";
            try
            {
                int compId = BL_Configdata.Focus8CompID;
                BL_Registry.SetLog("compId" + compId.ToString());
                string sessionID = GetSessionId(compId);
                BL_Registry.SetLog("sessionID" + sessionID.ToString());
                List<WIP_Transactions> trans = new List<WIP_Transactions>();
                trans = (List<WIP_Transactions>)TempData["listdata"];
                if (trans.Count > 0)
                {
                    BL_Registry.SetLog("Trans Count" + trans.Count.ToString());
                    string baseUrl = ConfigurationManager.AppSettings["Server_API_IP"];
                    int JVPostingFailed = 0;
                    trans = trans.Where(x=>x.count == x.rno).ToList();
                    string retrievequery = string.Format(@"exec pCore_CommonSp @Operation=getBaseCurrency");
                    int currencyid = Convert.ToInt32(DBClass.GetData(retrievequery, CompanyId, ref errors1).Tables[0].Rows[0][0].ToString());
                    BL_Registry.SetLog("currencyid" + currencyid.ToString());
                    foreach (var pay in trans)
                    {
                        int DrAC = -1;
                        if (pay.ML_id == pay.DivisionParentId)
                        {
                            if (pay.Balance < 0) // negative//debit balance
                            {
                                DrAC = pay.PL_ac;
                            }
                            else if (pay.Balance > 0)//positive// credit balance
                            {
                                DrAC = Convert.ToInt32(TempData["Account"]);
                            }
                        }
                        else
                        {
                            if (pay.Balance < 0) // negative//debit balance
                            {
                                DrAC = pay.CustomerId;
                            }
                            else if (pay.Balance > 0) //positive// credit balance
                            {
                                DrAC = Convert.ToInt32(TempData["Account"]);
                            }
                        }

                        int CrAC = -1;
                        if (pay.ML_id == pay.DivisionParentId)
                        {
                            if (pay.Balance < 0) // negative//debit balance
                            {
                                CrAC = Convert.ToInt32(TempData["Account"]);
                                
                            }
                            else if (pay.Balance > 0) //positive// credit balance
                            {
                                CrAC = pay.PL_ac;
                            }
                        }
                        else
                        {
                            if (pay.Balance < 0)// negative//debit balance
                            {
                                CrAC = Convert.ToInt32(TempData["Account"]);
                               
                            }
                            else if (pay.Balance > 0)//positive// credit balance
                            {
                                CrAC = pay.CustomerId;
                            }
                        }
                        
                        Hashtable headerJV = new Hashtable();
                        Hashtable objJVBody = new Hashtable();
                        List<Hashtable> listBodyJV = new List<Hashtable>();
                        headerJV.Add("Date", Convert.ToString(TempData["ReportDate"]));
                        BL_Registry.SetLog("Date" + Convert.ToString(TempData["ReportDate"]));
                        headerJV.Add("Currency", currencyid);
                        headerJV.Add("ExchangeRate", 1);
                        objJVBody.Add("Cost Center", pay.CostCenterID);
                        objJVBody.Add("Site", pay.SiteId);
                        objJVBody.Add("Project", pay.ProjectId);
                        objJVBody.Add("Division", pay.DivisionId);
                        objJVBody.Add("Activity", 156);//NA
                        objJVBody.Add("Service Code", 1276);//NA
                        objJVBody.Add("Employee Code", 25);//NA
                        objJVBody.Add("Fixed Asset", 247);//NA
                        objJVBody.Add("Reference", "");
                        objJVBody.Add("DrAccount", DrAC);
                        objJVBody.Add("CrAccount", CrAC);
                        objJVBody.Add("Amount", Math.Abs(pay.Balance));
                        listBodyJV.Add(objJVBody);

                        var postingData1 = new PostingData();
                        postingData1.data.Add(new Hashtable { { "Header", headerJV }, { "Body", listBodyJV } });
                        string sContent1 = JsonConvert.SerializeObject(postingData1);
                        string err1 = "";
                        string Url1 = baseUrl + "/Transactions/Vouchers/ JV WIP Reversal";
                        var response1 = Focus8API.Post(Url1, sContent1, sessionID, ref err1);
                        if (response1 != null)
                        {
                            BL_Registry.SetLog("posting Response" + response1.ToString());
                            var responseData1 = JsonConvert.DeserializeObject<APIResponse.PostResponse>(response1);
                            if (responseData1.result == -1)
                            {
                                BL_Registry.SetLog("posting Response failed" + responseData1.result.ToString());
                                JVPostingFailed++;
                                Message = "JV WIP Reversal Entry Posting Failed" + "\n";
                                BL_Registry.SetLog("JV WIP Reversal Entry Posted Failed with CostCenter: " + pay.CostCenter + " Site: " + pay.Site + " Doc No : " + pay.DocNo);
                                BL_Registry.SetLog2(response1 + "\n " + "POS Jounral Entry Posted Failed with CostCenter: " + pay.CostCenter + " Site: " + pay.Site + " Doc No : " + pay.DocNo + " \n Error Message : " + responseData1.message + "\n " + err1);
                            }
                            else
                            {
                                Message = "JV WIP Reversal Entry Posted Successfully" + "\n";
                                BL_Registry.SetLog("JV WIP Reversal Entry Posted Success with CostCenter: " + pay.CostCenter + " Site: " + pay.Site + " Doc No : " + pay.DocNo);
                            }
                        }
                    }

                    if(JVPostingFailed == 0)
                    {
                        //Hashtable objHash1 = new Hashtable();
                        //objHash1.Add("MasterId", trans[0].ProjectId);
                        //objHash1.Add("ProjectStatus", 1);
                        //objHash1.Add("ProjectClosureDate", Convert.ToString(TempData["ReportDate"]));
                        string sqlquery = string.Format($@"exec pCore_CommonSp @Operation=updateProjectStatus, @p1={trans[0].ProjectId},@p3='{Convert.ToString(TempData["ReportDate"])}'");
                        int a = DBClass.GetExecute(sqlquery, compId, ref errors1);
                        if (a == 1)
                        {
                            Message = "Project Updation done Successfully" + "\n";
                            BL_Registry.SetLog("Project Updation Success with MasterId: " + trans[0].ProjectId);
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Message = "Project Updation Failed" + "\n";
                            BL_Registry.SetLog("Project Updation Failed with MasterId: " + trans[0].ProjectId);
                            BL_Registry.SetLog2( "\n Project Updation Failed with MasterId: " + trans[0].ProjectId);
                            return Json("Error," + "Project Updation Failed", JsonRequestBehavior.AllowGet);
                        }
                        //var postingData = new PostingData();
                        //postingData.data.Add(objHash1);
                        //string errText = "";
                        //string sContent = JsonConvert.SerializeObject(postingData);
                        //string s = Focus8API.Post(baseUrl + "/Masters/Core__Project", sContent, sessionID, ref errText);
                        //if (s != null)
                        //{
                        //    var res = JsonConvert.DeserializeObject<APIResponse.PostResponse>(s);
                        //    if (res.result == -1)
                        //    {

                        //        Message = "Project Updation Failed" + "\n";
                        //        BL_Registry.SetLog("Project Updation Failed with MasterId: " + trans[0].ProjectId);
                        //        BL_Registry.SetLog2(res + "\n " + "Project Updation Failed with MasterId: " + trans[0].ProjectId + " \n Error Message : " + res.message);
                        //    }
                        //    else
                        //    {
                        //        Message = "Project Updation done Successfully" + "\n";
                        //        BL_Registry.SetLog("Project Updation Success with MasterId: " + trans[0].ProjectId);
                        //    }
                        //}
                    }
                    else
                    {
                        return Json("Error," + "Something went wrong in posting", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Error," + "Something went wrong", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json("Error,"+ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public class HashData
        {
            public string url { get; set; }
            public List<Hashtable> data { get; set; }
            public int result { get; set; }
            public string message { get; set; }
        }
        public partial class Datum
        {
            [JsonProperty("fSessionId")]
            public string FSessionId { get; set; }
        }
        public string getServiceLink()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string strFileName = "";
            string sAppPath = BL_Configdata.Focus8Path;
            strFileName = sAppPath + "\\ERPXML\\ServerSettings.xml";

            xmlDoc.Load(strFileName);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/ServSetting/MasterServer/ServerName");
            string strValue;
            XmlNode node = nodeList[0];
            if (node != null)
                strValue = node.InnerText;
            else
                strValue = "";
            return strValue;
        }
        public string GetSessionId(int CompId)
        {
            string sSessionId = "";
            try
            {
                string strServer = getServiceLink();
                BL_Registry.SetLog("strServer " + strServer);
                int ccode = CompId;
                string User_Name = BL_Configdata.UserName;
                string Password = BL_Configdata.Password;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + strServer + "/focus8api/Login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{" + "\"data\": [{" + "\"Username\":\"" + User_Name + "\"," + "\"password\":\"" + Password + "\"," + "\"CompanyId\":\"" + ccode + "\"}]}";
                    streamWriter.Write(json);
                    BL_Registry.SetLog("json " + json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader Updatereader = new StreamReader(httpResponse.GetResponseStream());
                string Udtcontent = Updatereader.ReadToEnd();

                JObject odtbj = JObject.Parse(Udtcontent);
                Temperatures Updtresult = JsonConvert.DeserializeObject<Temperatures>(Udtcontent);
                BL_Registry.SetLog("updateresult "+Updtresult.Result.ToString());
                if (Updtresult.Result == 1)
                {
                    sSessionId = Updtresult.Data[0].FSessionId;
                }


                return sSessionId;
            }
            catch (Exception ex)
            {
                BL_Registry.SetLog(ex.ToString());
            }
            return sSessionId;
        }
        public partial class Temperatures
        {
            [JsonProperty("data")]
            public Datum[] Data { get; set; }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("result")]
            public long Result { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
        [HttpPost]
        public FileResult ExcelGenerate()
        {

            #region TempData
            int CompanyId = Convert.ToInt32(TempData["CompanyId"]);
            string ReportDate = Convert.ToString(TempData["ReportDate"]);
            int Project = Convert.ToInt32(TempData["Project"]);
            string ProjectName = Convert.ToString(TempData["ProjectName"]);
            DateTime reportDt = Convert.ToDateTime(ReportDate);
            List<WIP_Transactions> trans = new List<WIP_Transactions>();
            trans = (List<WIP_Transactions>)TempData["listdata"];
            TempData.Keep();
            #endregion

            System.Data.DataTable data = new System.Data.DataTable("PROJECT CLOSURE");
            #region DataColumns
            data.Columns.Add("Sn", typeof(string));
            data.Columns.Add("Category", typeof(string));
            data.Columns.Add("Budget", typeof(string));
            data.Columns.Add("Non PO's", typeof(string));
            data.Columns.Add("PO's", typeof(string));
            data.Columns.Add("Forecast", typeof(string));
            data.Columns.Add("Save/(Loss)", typeof(string));

            #endregion


            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("PROJECT CLOSURE");
                var dataTable = data;



                var wsReportNameHeaderRange = ws.Range(ws.Cell(1, 2), ws.Cell(1, 8));//ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dataTable.Columns.Count + 1)));
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                wsReportNameHeaderRange.Style.Fill.BackgroundColor = XLColor.Yellow;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "                                                                             PROJECT CLOSURE          ";

                int cell = 2;
                for (int i = 1; i < data.Columns.Count; i++)
                {
                    cell = 2;
                    #region Headers
                    ws.Cell(4, cell++).Value = "Cost Center";
                    ws.Cell(4, cell++).Value = "Site";
                    ws.Cell(4, cell++).Value = "Doc No";
                    ws.Cell(4, cell++).Value = "Date";
                    ws.Cell(4, cell++).Value = "Dr";
                    ws.Cell(4, cell++).Value = "Cr";
                    ws.Cell(4, cell++).Value = "Balance";

                    #endregion
                }
                var TableRange = ws.Range(ws.Cell(4, 2), ws.Cell(4, 8));
                TableRange.Style.Font.FontColor = XLColor.White;
                TableRange.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 115, 170);
                TableRange.Style.Font.Bold = true;
                TableRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int r = 5;
                int c = 2;

                #region TableLoop
                var list = trans;
                decimal TotalReversal = 0;
                int count = 1;
                foreach (var obj in list)
                {
                    //count++;
                    c = 2;
                    ws.Range(ws.Cell(r, c), ws.Cell(r, 8)).Style.Fill.BackgroundColor = XLColor.FromHtml("#90EE90");
                    ws.Cell(r, c++).Value = obj.CostCenter;
                    ws.Cell(r, c++).Value = obj.Site;
                    ws.Cell(r, c++).Value = obj.DocNo;
                    ws.Cell(r, c++).Value = "'" + obj.Date.ToString().Split(' ')[0];
                    ws.Cell(r, c++).Value = obj.Dr.ToString("N", new CultureInfo("en-US"));
                    ws.Cell(r, c++).Value = obj.Cr.ToString("N", new CultureInfo("en-US"));

                    if (obj.count == obj.rno)
                    {
                        ws.Cell(r, c++).Value = obj.Balance.ToString("N", new CultureInfo("en-US"));
                        TotalReversal = Convert.ToDecimal(obj.Balance) + TotalReversal;
                    }
                    else
                    {
                        ws.Cell(r, c++).Value = "";
                    }

                    r++;
                    if (obj.count == obj.rno)
                    {
                        c = 2;
                        ws.Range(ws.Cell(r, c), ws.Cell(r, 8)).Style.Fill.BackgroundColor = XLColor.FromHtml("#90EE90");
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        r++;
                        c = 2;
                        ws.Range(ws.Cell(r, c), ws.Cell(r, 8)).Style.Fill.BackgroundColor = XLColor.FromHtml("#90EE90");
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        ws.Cell(r, c++).Value = "";
                        r++;
                    }
                }


                //Grand Total Row
                c = 2;
                ws.Range(ws.Cell(r, c++), ws.Cell(r, c+4)).Merge().Value= "Total Value for Reversal";
                ws.Cell(r, c+5).Value = TotalReversal.ToString("N", new CultureInfo("en-US"));

                ws.Range("B" + r + ":Z" + r + "").Style.Font.Bold = true;
                r++;


                #endregion

                TableRange = ws.Range(ws.Cell(4, 2), ws.Cell(r - 1, 8));
                TableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                TableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                TableRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range(ws.Cell(4, 3), ws.Cell(r, 8)).Style.NumberFormat.Format = "0.00";

                ws.Columns("A:BZ").AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project_Closure" + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx");
                }
            }
        }
    }
}