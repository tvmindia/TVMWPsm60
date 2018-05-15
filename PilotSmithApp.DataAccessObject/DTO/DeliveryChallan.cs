using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class DeliveryChallan
    {
        public Guid ID { get; set; }
        public string DelvChallanNo { get; set; }
        public string DelvChallanRefNo { get; set; }
        public DateTime DelvChallanDate { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? ProdOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public int? PlantCode { get; set; }
        public Guid? PreparedBy { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalIDv { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public int? BranchCode { get; set; }
        public string VehiclePlateNo { get; set; }
        public string DriverName { get; set; }

        //Additional Fields
        public string DelvChallanDateFormatted { get; set; }
        public string DetailXML { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public string[] DocumentOwners { get; set; }
        public List<DeliveryChallanDetail> DeliveryChallanDetailList { get; set; }
        public PSASysCommon PSASysCommon { get; set; }       
        public SaleOrder SaleOrder { get; set; }
        public ProductionOrder ProductionOrder { get; set; }
        public Customer Customer { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
        public Employee Employee { get; set; }
        public string DocumentType { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
    }

    public class DeliveryChallanAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }

    public class DeliveryChallanDetail
    {
        public Guid ID { get; set; }
        public Guid DelvChallanID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? DelvQty { get; set; }
        public int? UnitCode { get; set; }
        public Guid SpecTag { get; set; }

        //Aitional Fields
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        
    }
}
