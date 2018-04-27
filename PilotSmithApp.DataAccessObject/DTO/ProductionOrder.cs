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

        //Aditional Fields
        public string ProdOrderDateFormatted { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public List<ProductionOrderDetail> ProductionOrderDetailList { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Customer Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
    }

    public class ProductionOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
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

        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
    }
}
