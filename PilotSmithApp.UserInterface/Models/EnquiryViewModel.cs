using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class EnquiryViewModel
    {
        public Guid ID { get; set; }
        [Display(Name ="Enquiry No")]
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        [Display(Name = "Requirement Specification")]
        public string RequirementSpec { get; set; }
        [Required(ErrorMessage ="Customer is missing")]
        [Display(Name = "Select Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Grade")]
        public int? EnquiryGradeCode { get; set; }
        [Display(Name = "Status")]
        public int? DocumentStatusCode { get; set; }
        [Display(Name = "Referred By")]
        public int? ReferredByCode { get; set; }
        [Display(Name = "Responsible Person")]
        public Guid? ResponsiblePersonID { get; set; }
        [Display(Name = "Attended By")]
        public Guid? AttendedByID { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Required(ErrorMessage ="Branch Code is missing")]
        [Display(Name = "Branch")]
        public int? BranchCode { get; set; }
        //Additional properties
        public string DetailJSON { get; set; }
        [Display(Name ="Enquiry Date")]
        [Required(ErrorMessage ="Enquiry Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)",ErrorMessage ="Date format not accepted")]
        public string EnquiryDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        [Display(Name ="Document Locked")]
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public EnquiryGradeViewModel EnquiryGrade { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public AreaViewModel Area { get; set;}
        public List<EnquiryDetailViewModel> EnquiryDetailList { get; set; }
        public List<SelectListItem> EnquirySelectList { get; set; }
    }
    public class EnquiryAdvanceSearchViewModel
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        [Display(Name ="Enquiry From")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Enquiry To")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
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
    public class EnquiryDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        [Display(Name ="Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Specification")]
        public string ProductSpec { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage ="Quantity is missing")]
        public decimal? Qty { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "Unit is missing")]
        public int? UnitCode { get; set; }
        [Required(ErrorMessage = "Rate is missing")]
        public decimal? Rate { get; set; }
        public bool IsUpdate { get; set; }
        public Guid SpecTag { get; set; }
        public ProductViewModel Product { get; set; }     
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }

    public class EnquirySummaryViewModel
    {
        public int TotalEnquiryCount { get; set; }
        public int ConvertedEnquiryCount { get; set; }
    }
    public class EnquiryValueFolloupSummaryViewModel
    {
        public string Enquiry { get; set; }
        public decimal EnquiryValue { get; set; }
        public int FollowupCount { get; set; }
    }
}