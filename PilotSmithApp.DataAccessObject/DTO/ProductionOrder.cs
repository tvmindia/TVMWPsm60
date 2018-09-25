using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ProductionOrder
    {
        public Guid ID { get; set; }
        public string ProdOrderNo { get; set; }
        public string ProdOrderRefNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public Guid? PreparedBy { get; set; }
        public int? DocumentStatusCode { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public int? BranchCode { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }

        //Aditional Fields
        public string MailFrom { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public List<ProductionOrderDetail> ProductionOrderDetailList { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public Customer Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public int? ApproverLevel { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
        public string MailContant { get; set; }
        public Area Area { get; set; }
        public PSAUser PSAUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }

    public class ProductionOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid AdvCustomerID { get; set; }       
        public int? AdvAreaCode { get; set; }            
        public int? AdvBranchCode { get; set; }      
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }       
        public int? AdvApprovalStatusCode { get; set; }
        public string AdvEmailSentStatus { get; set; }
    }

    public class ProductionOrderDetail
    {
        public Guid ID { get; set; }
        public Guid ProdOrderID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ProducedQty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? MileStone1FcFinishDt { get; set; }
        public DateTime? MileStone1AcTFinishDt { get; set; }
        public DateTime? MileStone2FcFinishDt { get; set; }
        public DateTime? MileStone2AcTFinishDt { get; set; }
        public DateTime? MileStone3FcFinishDt { get; set; }
        public DateTime? MileStone3AcTFinishDt { get; set; }
        public DateTime? MileStone4FcFinishDt { get; set; }
        public DateTime? MileStone4AcTFinishDt { get; set; }
        public int? PlantCode { get; set; }
        //Additional fields
        public string MileStone1FcFinishDtFormatted { get; set; }
        public string MileStone1AcTFinishDtFormatted { get; set; }
        public string MileStone2FcFinishDtFormatted { get; set; }
        public string MileStone2AcTFinishDtFormatted { get; set; }
        public string MileStone3FcFinishDtFormatted { get; set; }
        public string MileStone3AcTFinishDtFormatted { get; set; }
        public string MileStone4FcFinishDtFormatted { get; set; }
        public string MileStone4AcTFinishDtFormatted { get; set; }
        public Guid SpecTag { get; set; }
        public decimal QCCompletedQty { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        public decimal? PrevProducedQty { get; set; }
        public decimal? PrevProdOrderQty { get; set; }
        public decimal? TotalProdOrderQty { get; set; }
        public decimal? TotalProdusedQty { get; set; }
        public Plant Plant { get; set; }
        public SaleOrderDetail SaleOrderDetail { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid SaleOrderDetailID { get; set; }
        public decimal? PrevDelQty { get; set; }
        public decimal? DelvQty { get; set; }
    }
    public class ProductionOrderSummary
    {
        public int TotalProductionOrderCount { get; set; }
        public int OpenProductionOrderCount { get; set; }
        public int ClosedProductionOrderCount { get; set; }
        public int InProgressProductionOrderCount { get; set; }
    }
}
