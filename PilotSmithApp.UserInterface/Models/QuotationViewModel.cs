using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class QuotationViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Quotation No.")]
        public string QuoteNo { get; set; }
        [Display(Name = "Quotation Ref No.")]
        [StringLength(20, ErrorMessage = "Ref.No Cannot be longer than 20 characters")]
        public string QuoteRefNo { get; set; }
        public DateTime QuoteDate { get; set; }
        [Display(Name = "Estimate")]
        [Required(ErrorMessage ="Estimate is missing")]
        public Guid? EstimateID { get; set; }
        [Display(Name = "Select Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Select Docuent Status")]
        public int? DocumentStatusCode { get; set; }
        public DateTime? ValidUpToDate { get; set; }
        [Display(Name = "Select Referred By")]
        public int? ReferredByCode { get; set; }
        [Display(Name = "Select Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name ="Quotation Header")]
        public string MailBodyHeader { get; set; }
        [Display(Name = "Quotation Footer")]
        public string MailBodyFooter { get; set; }
        [Display(Name = "Email Sent")]
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        [Display(Name = "Approval Status")]
        public int? LatestApprovalStatus { get; set; }
        [Display(Name = "Final Approval")]
        public bool? IsFinalApproved { get; set; }
        //[Required(ErrorMessage = "Please specify at least one recipient")]
        [Display(Name = "Sent To")]
        public string EmailSentTo { get; set; }
        [Display(Name = "Term Ref. No")]
        [StringLength(25, ErrorMessage = "Term Ref.No Cannot be longer than 25 characters")]
        public string TermReferenceNo { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Select Branch")]
        public int? BranchCode { get; set; }
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }
        [Display(Name = "Currency code")]
        public string CurrencyCode { get; set; }
        [Display(Name = "Currency rate")]
        public decimal CurrencyRate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional fields

        public string Cc { get; set; }
        public string Bcc { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        [Display(Name = "Document Locked")]
        public bool IsDocLocked { get; set; }
        public string MailContant { get; set; }
        public bool EmailFlag { get; set; }
        [AllowHtml]
        public string DetailJSON { get; set; }
        public string OtherChargesDetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        [Required(ErrorMessage = "Quotation Date is missing")]
        [Display(Name = "Quotation Date")]
        public string QuoteDateFormatted { get; set; }
        [Display(Name = "Valid Till Date")]
        [Required(ErrorMessage = "Valid Till Date is missing")]
        public string ValidUpToDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public int? ApproverLevel { get; set; }
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public List<QuotationDetailViewModel> QuotationDetailList { get; set; }
        public List<QuotationOtherChargeViewModel> QuotationOtherChargeList { get; set; }
        public PDFToolsViewModel PDFTools { get; set; }
        public List<SelectListItem> QuotationSelectList { get; set; }
        public List<SelectListItem> EstimateSelectList { get; set; }
        public AreaViewModel Area { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public string EstimateNo { get; set; }
        public bool ImageCheck { get; set; }
        public bool HeaderCheck { get; set; } = true;
        public bool PrintFlag { get; set; }
        public bool IsPrint { get; set; }
        public Guid? CopyFrom { get; set; }
        public string CopyQuoteNo { get; set; }
        public int IsFileExist { get; set; }

        public CurrencyViewModel Currency { get; set; }
        public bool IsDocumentApprover { get; set; }
    }
    public class QuotationAdvanceSearchViewModel
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        [Display(Name = "Quotation From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Quotation To")]
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
    public class QuotationDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid QuoteID { get; set; }
        [Display(Name = "Select Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Select Product Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
        public string ProductSpec { get; set; }
        [AllowHtml]
        public string ProductSpecHtml { get; set; }
        [Display(Name = "Quantity")]
        [Remote(action: "CheckQty", controller: "Quotation", AdditionalFields = "Qty")]
        [Required(ErrorMessage ="Quantity is missing")]
        public decimal? Qty { get; set; }
        [Display(Name = "Select Unit")]
        [Required(ErrorMessage = "Unit is missing")]
        public int? UnitCode { get; set; }
        [Remote(action: "CheckRate", controller: "Quotation", AdditionalFields = "Rate")]
        [Required(ErrorMessage = "Rate is missing")]
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        [Display(Name = "Select Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST Amount")]
        public decimal? CGSTPerc { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal? SGSTPerc { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal? IGSTPerc { get; set; }
        public bool IsUpdate { get; set; }
        public Guid SpecTag { get; set; }
        public string ImageURL { get;  set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
    }
    public class QuotationOtherChargeViewModel
    {
        public Guid ID { get; set; }
        public Guid QuoteID { get; set; }
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

        //Additional fields
        public bool IsUpdate { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
        public OtherChargeViewModel OtherCharge { get; set; }

    }
    public class QuotationSummaryViewModel
    {
        public int TotalQuotationCount { get; set; }
        public int ConvertedQuotationCount { get; set; }
        public int LostQuotationCount { get; set; }
    }
}