using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class SaleOrderViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Sale Order No.")]
        public string SaleOrderNo { get; set; }
        [Display(Name = "Sale Order Ref. No.")]
        [StringLength(20, ErrorMessage = "Ref.No Cannot be longer than 20 characters")]
        public string SaleOrderRefNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        [Display(Name = "Quotation")]
        public Guid? QuoteID { get; set; }
        [Display(Name = "Enquiry")]
        public Guid? EnquiryID { get; set; }
        [Display(Name = "Select Customer")]
        [Required(ErrorMessage = "Customer is missing")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Select Document Status")]
        public int? DocumentStatusCode { get; set; }
        public DateTime ExpectedDelvDate { get; set; }
        [Display(Name = "Select Reffered By")]
        public int? ReferredByCode { get; set; }
        [Display(Name = "Select Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name = "Purchase Order No.")]
        public string PurchaseOrdNo { get; set; }
        public DateTime PurchaseOrdDate { get; set; }
        [Display(Name = "Select Bank")]
        public int? BankCode { get; set; }
        [Display(Name = "Select Carrier")]
        public int? CarrierCode { get; set; }
        public bool? EmailSentYN { get; set; }
        [Display(Name = "Latest Approval")]
        public Guid LatestApprovalID { get; set; }
        [Display(Name = "Latest Approval Status")]
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        //[Required(ErrorMessage = "Please specify at least one recipient")]
        [Display(Name = "Sent To")]
        public string EmailSentTo { get; set; }
        [Display(Name = "Term Reference No.")]
        [StringLength(25, ErrorMessage = "Term Ref.No Cannot be longer than 25 characters")]
        public string TermReferenceNo { get; set; }
        [Display(Name = "Print Remark")]
        public string PrintRemark { get; set; }
        public string GeneralNotes { get; set; }
        [Display(Name = "Select Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Select Branch")]
        public int? BranchCode { get; set; }
        public decimal Discount { get; set; }
        [Display(Name = "Advance Amount")]
        public decimal? AdvanceAmount { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        //[Required(ErrorMessage = "subject is missing")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Display(Name = "Currency code")]
        public string CurrencyCode { get; set; }
        [Display(Name = "Currency rate")]
        public decimal CurrencyRate { get; set; }
        //Aditional Fields

        public string MailFrom { get; set; }
        public string MailBodyFooter { get; set; }
        public string MailContant { get; set; }
        public bool EmailFlag { get; set; }
        public string DocumentType { get; set; }
        public bool IsDocLocked { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SaleOrderDetailViewModel> SaleOrderDetailList { get; set; }
        public List<SaleOrderOtherChargeViewModel> SaleOrderOtherChargeList { get; set; }
        public string DetailJSON { get; set; }
        public string OtherChargesDetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Sale Order Date")]
        [Required(ErrorMessage = "SaleOrder Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string SaleOrderDateFormatted { get; set; }
        [Display(Name = "Expected Delivery Date")]
        [Required(ErrorMessage = "Expected Delivery Date is missing")]
        [RegularExpression("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)", ErrorMessage = "Date format not accepted")]
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Purchase Order Date")]
        public string PurchaseOrdDateFormatted { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public int? ApproverLevel { get; set; }
        public BranchViewModel Branch { get; set; }
        public CarrierViewModel Carrier { get; set; }
        public BankViewModel Bank { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public PDFToolsViewModel PDFTools { get; set; }
        public List<SelectListItem> QuotationSelectList{ get; set; }
        public List<SelectListItem> EnquirySelectList { get; set; }
        public string SaleOrderAmountWords { get; set; }
        public AreaViewModel Area { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public QuotationViewModel Quotation { get; set; }
        public EnquiryViewModel Enquiry { get; set; }
        public bool IsPrint { get; set; }
        public Guid? CopyFrom { get; set; }
        public string CopySaleOrderNo { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public int IsFileExist { get; set; }
    }

    public class SaleOrderAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name = "Sale Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Sale Order To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }     
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }       
        [Display(Name = "Referred By ")]
        public int? AdvReferencePersonCode { get; set; }       
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }      
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }       
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Email Sent(Y/N)")]
        public string AdvEmailSentStatus { get; set; }
    }

    public class SaleOrderDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        [Display(Name = "Select Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Select Product Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
        public string ProductSpec { get; set; }      
        [Display(Name = "Quantity")]
        [Remote(action: "CheckQty",controller:"SaleOrder",AdditionalFields = "Qty")]
        [Required(ErrorMessage = "Quantity is missing")]
        public decimal? Qty { get; set; }
        [Display(Name = "Select Unit")]
        public int? UnitCode { get; set; }
        [Remote(action:"CheckRate",controller:"SaleOrder",AdditionalFields ="Rate")]
        [Required(ErrorMessage ="Rate is missing")]
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        [Display(Name = "Select Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST")]
        public decimal? CGSTPerc { get; set; }
        [Display(Name = "SGST")]
        public decimal? SGSTPerc { get; set; }
        [Display(Name = "IGST")]
        public decimal? IGSTPerc { get; set; }
        [Display(Name = "Cess Percentage")]
        public decimal? CessPerc { get; set; }
        [Display(Name = "Cess Amount")]
        public decimal? CessAmt { get; set; }
        public Guid SpecTag { get; set; }

        //Additional Fields        
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
        public decimal? PrevProduceQty { get; set; }
        public decimal? PrevProdOrderQty { get; set; }
        public decimal? PrevDelQty { get; set; }
        public decimal? DelvQty { get; set; }
    }
    public class SaleOrderOtherChargeViewModel
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        [Display(Name = "Select Other Charge")]
        public int? OtherChargeCode { get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Charge Amount is missing")]
        public decimal? ChargeAmount { get; set; }
        [Display(Name = "Select Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST Amount")]
        public decimal CGSTPerc { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal SGSTPerc { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal IGSTPerc { get; set; }
        [Display(Name = "Additional Tax Perc")]
        public decimal? AddlTaxPerc { get; set; }
        [Display(Name = "Additional Tax Amount")]
        public decimal? AddlTaxAmt { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }

        //Additional fields
        public bool IsUpdate { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
        public OtherChargeViewModel OtherCharge { get; set; }

    }

    public class SaleOrderSummaryViewModel
    {
        public int TotalSaleOrderCount { get; set; }
        public int OpenSaleOrderCount { get; set; }
    }
}