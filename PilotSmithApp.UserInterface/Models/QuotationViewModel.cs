﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class QuotationViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Quotation Number")]
        public string QuoteNo { get; set; }
        [Display(Name = "Quotation Ref. Number")]
        public string QuoteRefNo { get; set; }
        public DateTime QuoteDate { get; set; }
        [Display(Name = "Estimate")]
        public Guid? EstimateID { get; set; }
        [Display(Name ="Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Status")]
        public int? DocumentStatusCode { get; set; }
        public DateTime? ValidUpToDate { get; set; }
        [Display(Name = "Referred By")]
        public int? ReferredByCode { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name ="Mail Body Header")]
        public string MailBodyHeader { get; set; }
        [Display(Name = "Mail Footer Header")]
        public string MailBodyFooter { get; set; }
        public bool? EmailSentYN { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        [Required(ErrorMessage = "Please specify at least one recipient.")]
        public string EmailSentTo { get; set; }
        public string TermReferenceNo { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Branch")]
        public int? BranchCode { get; set; }
        [Display(Name = "Discount")]
        public decimal? Discount { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional fields
        
        public string MailContant { get; set; }
        public bool EmailFlag { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        [Required(ErrorMessage = "Quotation Date is missing")]
        [Display(Name = "Quotation Date")]
        public string QuoteDateFormatted { get; set; }
        [Display(Name = "Valid Till Date")]
        public string ValidUpToDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        public List<QuotationDetailViewModel> QuotationDetailList { get; set; }
        public PDFTools PDFTools { get; set; }
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
        [Display(Name ="Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Model")]
        public Guid? ProductModelID { get; set; }
        [Display(Name = "Product Specification")]
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
        public decimal? CGSTAmt { get; set; }
        [Display(Name = "SGST Amount")]
        public decimal? SGSTAmt { get; set; }
        [Display(Name = "IGST Amount")]
        public decimal? IGSTAmt { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
    }
}