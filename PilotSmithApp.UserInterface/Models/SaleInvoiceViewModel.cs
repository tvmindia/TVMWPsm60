using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class SaleInvoiceViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Invoice No")]
        public string SaleInvNo { get; set; }
        [Display(Name = "Reference No")]
        [StringLength(20, ErrorMessage = "Ref.No Cannot be longer than 20 characters")]
        public string SaleInvRefNo { get; set; }
        public DateTime SaleInvDate { get; set; }
        public Guid? QuoteID { get; set; }
        public Guid? SaleOrderID { get; set; }
        [Display(Name = "Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Document Status")]
        public int? DocumentStatusCode { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public bool? CashInvoiceYN { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        public string PurchaseOrdNo { get; set; }
        public DateTime? PurchaseOrdDate { get; set; }
        public int? BillSeriesCode { get; set; }
        [Display(Name = "Email Sent")]
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public string PrintRemark { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Branch")]
        public int? BranchCode { get; set; }
        [Display(Name = "Billing Location")]
        public int? BillLocationCode { get; set; }
        [Display(Name = "Invocie Type")]
        public string InvocieType { get; set; }
        public decimal? Discount { get; set; }
        public decimal? AdvanceAmount { get; set; }
        //additional Fields
        public string DocumentType { get; set; }
        public string DetailJSON { get; set; }
        public string OtherChargesDetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        [Display(Name = "Invoice Date")]

        [Required(ErrorMessage = "Invoice Date is missing")]
        public string SaleInvDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDocLocked { get; set; }
        public string MailFrom { get; set; }
        public string MailBodyFooter { get; set; }
        public string MailContant { get; set; }
        public string SignatureStamp { get; set; }
        public string SignatureStampLine2 { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyAddress3 { get; set; }
        public string GSTIN { get; set; }
        public string CIN { get; set; }
        public string PAN { get; set; }
        public string EmailID { get; set; }
        public bool EmailFlag { get; set; } 
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        [Display(Name = "Expected Delivery Date")]
        [Required(ErrorMessage = "Expected Delivery Date is missing")]
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Purchase Order Date")]
        public string PurchaseOrdDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public BranchViewModel Branch { get; set; }
        public BillLocationViewModel BillLocation { get; set; }
        public List<SaleInvoiceDetailViewModel> SaleInvoiceDetailList { get; set; }
        public List<SaleInvoiceOtherChargeViewModel> SaleInvoiceOtherChargeDetailList { get; set; }
        public List<SelectListItem> QuotationSelectList { get; set; }
        public List<SelectListItem> SaleOrderSelectList { get; set; }
        public AreaViewModel Area { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public string ReferenceNo { get; set; }
        public QuotationViewModel Quotation { get; set; }
        public SaleOrderViewModel SaleOrder { get; set; }
        public PDFToolsViewModel PDFTools { get; set; }
    }
    public class SaleInvoiceAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name = "Sale Invoice From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Sale Invoice To")]
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
        [Display(Name = "Document owner")]
        public Guid AdvDocumentOwnerID { get; set; }        
        [Display(Name = "Approval status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Email Sent (Y/N)")]
        public string AdvEmailSentStatus { get; set; }
    }
    public class SaleInvoiceDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid? SaleInvID { get; set; }
        [Display(Name = "Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Product Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Spec")]
        public string ProductSpec { get; set; }
        [Display(Name = "Quantity")]
        public decimal? Qty { get; set; }
        [Display(Name = "Unit")]
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        [Display(Name = "Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST Amount")]
        public decimal? CGSTPerc { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal? SGSTPerc { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal? IGSTPerc { get; set; }
        [Display(Name = "Cess Percentage")]
        public decimal? CessPerc { get; set; }
        [Display(Name = "Cess Amount")]
        public decimal? CessAmt { get; set; }
        [Display(Name = "Select Service Item")]
        public int? OtherChargeCode { get; set; }
        public OtherChargeViewModel OtherCharge { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
    }
    public class SaleInvoiceOtherChargeViewModel
    {
        public Guid ID { get; set; }
        public Guid SaleInvID { get; set; }
        [Display(Name = "Other Charge")]
        public int? OtherChargeCode { get; set; }
        [Required(ErrorMessage = "Charge Amount is missing")]
        [Display(Name = "Charge Amount")]
        public decimal? ChargeAmount { get; set; }
        [Display(Name = "Tax Type")]
        public int? TaxTypeCode { get; set; }
        [Display(Name = "CGST Amount")]
        public decimal CGSTPerc { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal SGSTPerc { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal IGSTPerc { get; set; }
        [Display(Name = "Additional Tax Percentage")]
        public decimal AddlTaxPerc { get; set; }
        [Display(Name = "Additional Tax Amount")]
        public decimal AddlTaxAmt { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional Fields
        public bool IsUpdate { get; set; }
        public OtherChargeViewModel OtherCharge { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
    }
}