using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class QuotationViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage ="Quotation Number is missing")]
        [Display(Name ="Quotation Number")]
        public string QuoteNo { get; set; }
        [Required(ErrorMessage = "Quotation Ref. Number is missing")]
        [Display(Name = "Quotation Ref. Number")]
        public string QuoteRefNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public Guid? EstimateID { get; set; }
        public Guid? CustomerID { get; set; }
        public string MailingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int? DocumentStatusCode { get; set; }
        public DateTime? ValidUpToDate { get; set; }
        public int? ReferredByCode { get; set; }
        public Guid? PreparedBy { get; set; }
        public string MailBodyHeader { get; set; }
        public string MailBodyFooter { get; set; }
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public string TermReferenceNo { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public decimal? Discount { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional fields
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        [Required(ErrorMessage = "Quotation Date is missing")]
        [Display(Name = "Quotation Date")]
        public string QuoteDateFormatted { get; set; }
        public string ValidUpToDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public List<QuotationDetailViewModel> QuotationDetailList { get; set; }
    }
    public class QuotationAdvanceSearchViewModel
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class QuotationDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid QuoteID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTAmt { get; set; }
        public decimal? SGSTAmt { get; set; }
        public decimal? IGSTAmt { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
    }
}