using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectClosure.Models
{
    public class TrialBalanceModel
    {
        public string sessionId { get; set; }
        public int Userid { get; set; }
        public List<Transactions> listTransactions { get; set; }
        public Search Search { get; set; }
    }
    public class Transactions
    {
        public string AccountType { get; set; }
        public string AccountGroup { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterCode { get; set; }
        public string Site { get; set; }
        public string SiteCode { get; set; }
        public string Project { get; set; }
        public string Activity { get; set; }
        public string ServiceCode { get; set; }
        public string Division { get; set; } 
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TranDr { get; set; }
        public decimal TranCr { get; set; }
        public decimal TranBal { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Bal { get; set; }
        
    }
    public class Search
    {
        public int Cid { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Project { get; set; }
        public string Division { get; set; }
        public string CostCenter { get; set; }
        public string Site { get; set; }
        public string Activity { get; set; }
        public string ServiceCode { get; set; }
        public string AccountGroup { get; set; }
        public string Account { get; set; }
        public string DivisionNames { get; set; }
        public string CostCenterNames { get; set; }
        public string AcIds { get; set; }
        public string AcNames { get; set; }
        public string AcGrpIds { get; set; }
        public string AcGrpNames { get; set; }
    }
    public class JqueryDatatableParam
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
    }
}