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
        public string Site { get; set; }
        public string Project { get; set; }
        public string Activity { get; set; }
        public string ServiceCode { get; set; }
        public string Division { get; set; } 
        public string Currency { get; set; }
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
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Project { get; set; }
        public string Division { get; set; }
        public string CostCenter { get; set; }
        public string Site { get; set; }
        public string Activity { get; set; }
        public string ServiceCode { get; set; }
    }
}