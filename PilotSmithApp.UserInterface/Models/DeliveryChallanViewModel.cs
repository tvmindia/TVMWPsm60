using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class DeliveryChallanViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Delivery challan No.")]
        public string DelvChallanNo { get; set; }
        [Display(Name = "Delivery challan Ref.No.")]
        [StringLength(20, ErrorMessage = "Ref.No Cannot be longer than 20 characters")]
        public string DelvChallanRefNo { get; set; }
        public DateTime DelvChallanDate { get; set; }
        [Display(Name = "Sale Order")]
        [Required(ErrorMessage ="Select Document")]
        public Guid? SaleOrderID { get; set; }
        [Display(Name = "Prouction Order")]
        [Required(ErrorMessage = "Select Document")]
        public Guid? ProdOrderID { get; set; }
        [Display(Name ="Select Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name ="Plant")]
        public int? PlantCode { get; set; }
        [Display(Name ="Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name ="General Notes")]
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name ="Email Sent")]
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalIDv { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        [Display(Name ="Branch")]
        [Required(ErrorMessage ="Branch is missing")]
        public int? BranchCode { get; set; }
        [Display(Name ="Vehicle Plate No.")]
        public string VehiclePlateNo { get; set; }
        [Display(Name ="Driver Name")]
        public string DriverName { get; set; }

        //Additional Fields
        [Display(Name ="Delivery challan Date")]
        [Required(ErrorMessage = "Delivery Challan Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string DelvChallanDateFormatted { get; set; }
        public string DetailJSON { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        [Display(Name = "Document Locked")]
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public List<DeliveryChallanDetailViewModel> DeliveryChallanDetailList { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public SaleOrderViewModel SaleOrder { get; set; }
        public ProductionOrderViewModel ProductionOrder { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public BranchViewModel Branch { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string DocumentType { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public List<SelectListItem> SaleOrderSelectList { get; set; }
        public List<SelectListItem> ProductionOrderSelectList { get; set; }
        public PlantViewModel Plant { get; set; }
        public bool SaleOrderValue { get; set; } //for radio button 
        public bool ProdOrderValue { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public AreaViewModel Area { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public string ReferenceNo { get; set; }
    }

    public class DeliveryChallanAdvanceSearchViewModel
    {
        
        public string SearchTerm { get; set; }
        [Display(Name ="From Date")]
        public string AdvFromDate { get; set; }
        [Display(Name = "To Date")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Email Sent (Y/N)")]
        public string AdvEmailSentStatus { get; set; }
        [Display(Name = "Supplier / Plant")]
        public int? AdvPlantCode { get; set; }
        public PlantViewModel Plant { get; set; }
    }

    public class DeliveryChallanDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid DelvChallanID { get; set; }
        [Display(Name ="Select Product")]
        [Required(ErrorMessage ="Product is missing")]
        public Guid? ProductID { get; set; }
        [Display(Name ="Product Model")]
        [Required(ErrorMessage = "Product Model is missing")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
        [Required(ErrorMessage = "Product Specification is missing")]
        public string ProductSpec { get; set; }
        [Display(Name ="Order Qty")]
        public decimal? OrderQty { get; set; }
        [Display(Name ="Delivery Qty")]
        public decimal? DelvQty { get; set; }
        [Display(Name ="Unit")]
        [Required(ErrorMessage ="Unit is missing")]
        public int? UnitCode { get; set; }
        public Guid SpecTag { get; set; }
        
        //Aitional Fields

        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public bool IsUpdate { get; set; }
        public decimal? PrevDelQty { get; set; }
        //public decimal? PrevDelQtySale { get; set; }
        //public decimal? PrevDelQtyProd { get; set; }
    }
}