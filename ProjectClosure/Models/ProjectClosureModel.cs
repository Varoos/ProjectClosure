using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectClosure.Models
{
    public class ProjectClosureModel
    {
        public string sessionId { get; set; }
        public int Userid { get; set; }
        public List<WIP_Transactions> listWIP_Transactions { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
    }
    public class WIP_Transactions
    {
        public string CostCenter { get; set; }
        public string Site { get; set; }
        public string DocNo { get; set; }
        public string Date { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public decimal Balance { get; set; }
        public int count { get; set; }
        public int rno { get; set; }
        public int CostCenterID { get; set; }
        public int SiteId { get; set; }
        public int DivisionId { get; set; }
        public int ProjectId { get; set; }
        public int ML_id { get; set; }
        public int DivisionParentId { get; set; }
        public int PL_ac { get; set; }
        public int CustomerId { get; set; }
    }
    public class SearchCriteria
    {
        public int Cid { get; set; }
        public string ClosureDate { get; set; }
        public string Project { get; set; }
        public string Division { get; set; }
        public string Customer { get; set; }
        public string WIPAc { get; set; }
        public int ProjectId { get; set; }
        public int DivisionId { get; set; }
        public int CustomerId { get; set; }
        public int WIPAcId { get; set; }
    }
}