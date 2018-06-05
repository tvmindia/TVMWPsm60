using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductionQCViewModel
    {
        public Guid ID { get; set; }
        [Display(Name ="Quality Control No")]
        public string ProdQCNo { get; set; }
        [Display(Name = "Quality Control Ref.No")]
        public string ProdQCRefNo { get; set; }
        public DateTime ProdQCDate { get; set; }
        [Required(ErrorMessage ="Product order is missing")]
        public Guid? ProdOrderID { get; set; }
        [Display(Name = "Select Customer")]
        [Required(ErrorMessage = "Customer is missing")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Select Plant")]
        public int? PlantCode { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name = "Select Status")]
        public int? DocumentStatusCode { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name ="Email")]
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        [Display (Name ="Final Approval")]
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        [Display(Name = "Select Branch")]
        public int? BranchCode { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //additional fields
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public string DetailJSON { get; set; }
        [Display(Name = "Quality Control Date")]
        [Required(ErrorMessage ="Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ProdQCDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public List<ProductionQCDetailViewModel> ProductionQCDetailList { get; set; }
        public CustomerViewModel Customer { get; set; }
        public List<SelectListItem> ProdOrderSelectList { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public AreaViewModel Area { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public BranchViewModel Branch { get; set; }
        public PlantViewModel Plant { get; set; }
        public string ProdOrderNo { get; set; }
    }
    public class ProductionQCAdvanceSearchViewModel
    {
        public string QCDate { get; set; }
        public string SearchTerm { get; set; }
        [Display(Name = "Production QC From")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string FromDate { get; set; }
        [Display(Name = "Production QC To")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ToDate { get; set; }
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
        [Display(Name = "EmailSent(Y/N)")]
        public string AdvEmailSentStatus { get; set; }
        [Display(Name = "Supplier / Plant")]
        public int? AdvPlantCode { get; set; }
        public PlantViewModel Plant { get; set; }
    }
    public class ProductionQCDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid ProdQCID { get; set; }
        [Display(Name ="Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Product Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
        public string ProductSpec { get; set; }
        [Display(Name ="QC Quantity")]
        public decimal? QCQty { get; set; }
        public DateTime QCDate { get; set; }
        [Required(ErrorMessage = "QC By is missing")]
        [Display(Name ="Select QC By")]
        public Guid? QCBy { get; set; }
        public Guid SpecTag { get; set; }
        //additional fields
        [Required(ErrorMessage ="QC Date is missing")]
        [Display(Name = "QC Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string QCDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public decimal? ProducedQty { get; set; }
        public decimal? QCQtyPrevious { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
}