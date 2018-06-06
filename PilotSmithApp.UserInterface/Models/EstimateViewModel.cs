using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class EstimateViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Estimate No.")]
        public string EstimateNo { get; set; }
        [Display(Name = "Estimate Ref No.")]  
        [StringLength(20,ErrorMessage ="Ref.No Cannot be longer than 20 characters")]      
        public string EstimateRefNo { get; set; }
        public DateTime EstimateDate { get; set; }
        [Display(Name = "Enquiry")]
        public Guid? EnquiryID { get; set; }
        [Display(Name = "Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Document Status")]
        [Required(ErrorMessage = "Documentstatus is missing")]
        public int? DocumentStatusCode { get; set; }
        [Display(Name = "Valid upto")]
        public DateTime ValidUpToDate { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name = "General notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Branch")]
        [Required(ErrorMessage ="Branch is missing")]
        public int? BranchCode { get; set; }

        //Aditional Fields
        public string DetailJSON { get; set; }
        [Display(Name = "Estimate Date")]
        [Required(ErrorMessage = "Estimate Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string EstimateDateFormatted { get; set; }
        [Display(Name ="Valid Upto")]
        [Required(ErrorMessage = "Valid Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ValidUpToDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        [Display(Name = "Document Locked")]
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public EnquiryViewModel Enquiry { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public BranchViewModel Branch { get; set; }
        public string UserName { get; set; }
        public List<EstimateDetailViewModel> EstimateDetailList { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<SelectListItem> EstimateSelectList { get; set; }
        public List<SelectListItem> EnquirySelectList { get; set; }
        public AreaViewModel Area { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
    }

    public class EstimateAdvanceSearchViewModel
    {
        public string EstimateDate { get; set; }
        public string SearchTerm { get; set; }
        [Display(Name = "Estimate From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Estimate To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Reffered By ")]
        public int? AdvReferencePersonCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
    }

    public class EstimateDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid EstimateID { get; set; }
        [Display(Name ="Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Product Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
        public string ProductSpec { get; set; }
        [Required(ErrorMessage ="Quantity is missing")]
        public decimal? Qty { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "Unit is missing")]
        public int? UnitCode { get; set; }
        [Display(Name = "Cost Rate")]
        public decimal? CostRate { get; set; }
        [Display(Name = "Selling Rate")]
        public decimal? SellingRate { get; set; }
        [Display(Name ="Drawing No.")]
        public string DrawingNo { get; set; }
        public Guid SpecTag { get; set; }

        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public bool IsUpdate { get; set; }
    }
}