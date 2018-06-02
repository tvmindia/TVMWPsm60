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
        public int? BranchCode { get; set; }
        public decimal? Discount { get; set; }
        public decimal? AdvanceAmount { get; set; }
        //additional Fields
        public string DocumentType { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        [Display(Name = "Invoice Date")]
        public string SaleInvDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        [Display(Name = "Expected Delivery Date")]
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Purchase Order Date")]
        public string PurchaseOrdDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public List<SaleInvoiceDetailViewModel> SaleInvoiceDetailList { get; set; }
        public List<SelectListItem> QuotationSelectList { get; set; }
        public List<SelectListItem> SaleOrderSelectList { get; set; }
    }
    public class SaleInvoiceAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class SaleInvoiceDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid? SaleInvID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTPerc { get; set; }
        public decimal? SGSTPerc { get; set; }
        public decimal? IGSTPerc { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessAmt { get; set; }
        //Additional Fields
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
        public int OtherChargeCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public int TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal AddlTaxPec { get; set; }
        public decimal AddlTaxAmt { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}