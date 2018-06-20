using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ServiceCallViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Service Call No")]
        public string ServiceCallNo { get; set; }
        public DateTime ServiceCallDate { get; set; }
        public DateTime? ServiceCallTime { get; set; }
        [Display(Name = "Select Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Attended By")]
        public Guid? AttendedBy { get; set; }
        [Display(Name = "Caller Name")]
        public string CalledPersonName { get; set; }
        [Display(Name = "Document Status")]
        public int? DocumentStatusCode { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Serviced By")]
        public Guid? ServicedBy { get; set; }
        public DateTime? ServiceDate { get; set; }
        [Display(Name = "Service Comments")]
        public string ServiceComments { get; set; }
        [Display(Name = "Branch")]
        public int? BranchCode { get; set; }
        public Guid? DocumentOwnerID { get; set; }

        //Additional Fields
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public bool IsDocLocked { get; set; }
        public List<ServiceCallDetailViewModel> ServiceCallDetailList { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        [Display(Name = "Service Call Date")]
        [Required(ErrorMessage = "Service Call Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)",ErrorMessage ="Date format not accepted")]
        public string ServiceCallDateFormatted { get; set; }
        [Display(Name = "Service Call Time")]
        public string ServiceCallTimeFormatted { get; set; }
        [Display(Name = "Service Date")]
        [Required(ErrorMessage = "Service Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)",ErrorMessage ="Date format not accepted")]
        public string ServiceDateFormatted { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public string CallChargeJSON { get; set; }
        public List<ServiceCallChargeViewModel> ServiceCallChargeList { get; set; }
        public string ServicedByName { get; set; }
        public BranchViewModel Branch { get; set; }
        public AreaViewModel Area { get; set; }
    }

    public class ServiceCallAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name = "Service From")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Service To")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string AdvToDate { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }
        [Display(Name = "Service By")]
        public Guid AdvServicedBy { get; set; }
        [Display(Name = "Attended By")]
        public Guid AdvAttendedBy { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        public CustomerViewModel AdvCustomer { get; set; }
        public AreaViewModel AdvArea { get; set; }
        public EmployeeViewModel AdvEmployee { get; set; }
        public EmployeeViewModel AdvServicedEmployee { get; set; }
        public DocumentStatusViewModel AdvDocumentStatus { get; set; }
        public BranchViewModel AdvBranch { get; set; }
    }

    public class ServiceCallDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid ServiceCallID{get;set;}
        [Display(Name ="Product")]
        public Guid? ProductID { get;set;}
        [Display(Name = "Model")]
        public Guid? ProductModelID { get;set;}
        [Display(Name = "Specification")]
        public string ProductSpec { get;set;}
        [Display(Name = "Guarantee Y/N")]
        public bool? GuaranteeYN { get;set;}
        public DateTime? InstalledDate { get;set;}
        [Display(Name = "Service Status")]
        public int? ServiceStatusCode { get;set;}
        public bool IsUpdate { get; set; }

        //Additional Field
        [Display(Name = "Installed Date")]
        public string InstalledDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
    }

    public class ServiceCallChargeViewModel
    {
        public Guid ID { get; set; }
        public Guid? ServiceCallID { get; set; }
        [Display(Name = "Other Charge")]
        public int? OtherChargeCode { get; set; }
        [Required(ErrorMessage = "Charge Amount is missing")]
        public decimal? ChargeAmount { get; set; }
        [Display(Name = "Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST Amount")]
        public decimal? CGSTPerc { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal? SGSTPerc { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal? IGSTPerc { get; set; }
        [Display(Name = "Additional Tax Percentage")]
        public decimal? AddlTaxPerc { get; set; }
        [Display(Name = "Additional Tax Amount")]
        public decimal? AddlTaxAmt { get; set; }

        //Additional Field
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
        public OtherChargeViewModel OtherCharge { get; set; }
    }
}
