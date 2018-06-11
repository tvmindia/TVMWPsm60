using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductionOrderViewModel
    {
        public Guid ID{get;set;}
        [Display(Name ="Production Order No.")]
        //[Required(ErrorMessage ="Production Order No. is missing")]
        public string ProdOrderNo{get;set;}
        [Display(Name = "Production Order Ref.No.")]
        [StringLength(20, ErrorMessage = "Ref.No Cannot be longer than 20 characters")]
        public string ProdOrderRefNo{get;set;}
        public DateTime ProdOrderDate{get;set;}
        [Display(Name ="Select Sale Order")]
        public Guid? SaleOrderID{get;set;}
        [Display(Name = "Select Customer")]       
        public Guid? CustomerID{get;set;}
        public DateTime? ExpectedDelvDate{get;set;}
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy{get;set;}
        [Display(Name = "Status")]
        [Required(ErrorMessage = "Documentstatus is missing")]
        public int? DocumentStatusCode{get;set;}
        [Display(Name = "General Notes")]
        public string GeneralNotes{get;set;}
        public Guid? DocumentOwnerID{get;set;}
        [Display(Name ="Email Sent")]
        public bool? EmailSentYN{get;set;}
        public Guid? LatestApprovalID{get;set;}
        [Display(Name = "Approval Status")]
        public int? LatestApprovalStatus{get;set;}
        [Display(Name = "Final Approval")]
        public bool? IsFinalApproved{get;set;}
        [Required(ErrorMessage = "Please specify at least one recipient.")]
        public string EmailSentTo{get;set;}
        [Display(Name ="Branch")]
        [Required(ErrorMessage ="Branch is missing")]
        public int? BranchCode{get;set;}

        //Aditional Fields

        public string MailFrom { get; set; }
        [Display(Name ="Production Order Date")]
        [Required(ErrorMessage ="Production Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ProdOrderDateFormatted { get; set; }
        [Display(Name = "Expected Delivery Date")]       
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ExpectedDelvDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Document Locked")]
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public BranchViewModel Branch { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public List<ProductionOrderDetailViewModel> ProductionOrderDetailList { get; set; }
        public List<SelectListItem> SaleOrderSelectList { get; set; }
        public string MailContant { get; set; }
        public bool EmailFlag { get; set; }
        public PDFToolsViewModel PDFTools { get; set; }
        public AreaViewModel Area { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
    }

    public class ProductionOrderAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name ="Production Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Production Order To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }       
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }         
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }        
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }       
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Email Sent (Y/N)")]
        public string AdvEmailSentStatus { get; set; }
    }

    public class ProductionOrderDetailViewModel
    {
        public Guid ID{get;set;}        
        public Guid ProdOrderID{get;set;}
        [Display(Name = "Product")]
        public Guid? ProductID{get;set;}
        [Display(Name = "Product Model")]
        public Guid? ProductModelID{get;set;}
        [Display(Name = "Product Specification")]
        [Required(ErrorMessage = "Product Specfication is missing")]
        public string ProductSpec{get;set;}
        [Display(Name ="Order Qty")]
        public decimal? OrderQty{get;set;}
        [Display(Name = "Produced Qty")]
        public decimal? ProducedQty{get;set;}       
        [Display(Name = "Unit")]
        public int? UnitCode{get;set;}
        [Display(Name = "Rate")]
        public decimal? Rate{get;set;}
        public DateTime? MileStone1FcFinishDt{get;set;}
        public DateTime? MileStone1AcTFinishDt{get;set;}
        public DateTime? MileStone2FcFinishDt{get;set;}
        public DateTime? MileStone2AcTFinishDt{get;set;}
        public DateTime? MileStone3FcFinishDt{get;set;}
        public DateTime? MileStone3AcTFinishDt{get;set;}
        public DateTime? MileStone4FcFinishDt{get;set;}
        public DateTime? MileStone4AcTFinishDt{get;set;}
        public int? PlantCode{get;set;}
        public Guid SpecTag { get; set; }
        //Additional Fields
        [Display(Name = "MileStone 1 ForeCast Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone1FcFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 1 Actual Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone1AcTFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 2 ForeCast Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone2FcFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 2 Actual Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone2AcTFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 3 ForeCast Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone3FcFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 3 Actual Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone3AcTFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 4 ForeCast Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone4FcFinishDtFormatted { get; set; }
        [Display(Name = "MileStone 4 Actual Finish Date")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string MileStone4AcTFinishDtFormatted { get; set; }
              
        public bool IsUpdate { get; set; }
        public decimal QCCompletedQty { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public decimal? PrevProducedQty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CurProducedQty { get; set; }
        public PlantViewModel Plant { get; set; }
        public decimal? PrevDelQty { get; set; }
        public decimal? DelvQty { get; set; }
    }

    public class ProductionOrderSummaryViewModel
    {
        public int TotalProductionOrderCount { get; set; }
        public int OpenProductionOrderCount { get; set; }
        public int ClosedProductionOrderCount { get; set; }
        public int InProgressProductionOrderCount { get; set; }
    }
}