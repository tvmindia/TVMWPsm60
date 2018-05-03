using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ProductionQC 
    {
        public Guid ID { get; set; }
        public string ProdQCNo { get; set; }
        public string ProdQCRefNo { get; set; }
        public DateTime ProdQCDate { get; set; }
        public Guid ProdOrderID { get; set; }
        public Guid CustomerID { get; set; }
        public int PlantCode { get; set; }
        public Guid PreparedBy { get; set; }
        public int DocumentStatusCode { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        public bool EmailSentYN { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public int BranchCode { get; set; }
        public PSASysCommon  PSASysCommon { get; set; }
        //additional fields
        public string DetailXML { get; set; }
        public string ProdQCDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public List<ProductionQCDetail> ProductionQCDetailList { get; set; }
        public Customer Customer { get; set; }
        public DocumentStatus DocumentStatus { get; set;}
    }
    public class ProductionQCAdvanceSearch
    {
        public string QCDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
    public class ProductionQCDetail 
    {
        public Guid ID { get; set; }
        public Guid ProdQCID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal QCQty { get; set; }
        public DateTime QCDate { get; set; }
        public Guid QCBy { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }

    }
}
