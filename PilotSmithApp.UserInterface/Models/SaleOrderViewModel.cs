using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class SaleOrderViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Sale Order No.")]
        public string SaleOrderNo { get; set; }
        [Display(Name = "Sale Order Ref. No.")]
        public string SaleOrderRefNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        [Display(Name = "Quotation")]
        public Guid? QuoteID { get; set; }
        [Display(Name = "Enquiry")]
        public Guid? EnquiryID { get; set; }
        [Display(Name = "Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Doc. Status")]
        public int? DocumentStatusCode { get; set; }
        public DateTime ExpectedDelvDate { get; set; }
        [Display(Name = "Reffered By")]
        public int? ReferredByCode { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name = "Purchase Order No.")]
        public string PurchaseOrdNo { get; set; }
        public DateTime PurchaseOrdDate { get; set; }
        [Display(Name = "Bank")]
        public int? BankCode { get; set; }
        [Display(Name = "Carrier")]
        public int? CarrierCode { get; set; }
        public bool? EmailSentYN { get; set; }
        [Display(Name = "Latest Approval")]
        public Guid LatestApprovalID { get; set; }
        [Display(Name = "Latest Approval Status")]
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        [Display(Name = "Sent To")]
        public string EmailSentTo { get; set; }
        [Display(Name = "Term Reference No.")]
        public string TermReferenceNo { get; set; }
        [Display(Name = "Print Remark")]
        public string PrintRemark { get; set; }
        public string GeneralNotes { get; set; }
        [Display(Name = "Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Branch")]
        public int? BranchCode { get; set; }
        public decimal? Discount { get; set; }
        [Display(Name = "Advance Amount")]
        public decimal? AdvanceAmount { get; set; }

        //Aditional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SaleOrderDetailViewModel> SaleOrderDetailList { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Sale Order Date")]
        [Required(ErrorMessage = "SaleOrder Date is missing")]
        public string SaleOrderDateFormatted { get; set; }
        [Display(Name = "Expt.Del Date")]
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Purchase Ord. Date")]
        public string PurchaseOrdDateFormatted { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public BranchViewModel Branch { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
    }

    public class SaleOrderAdvanceSearchViewModel
    {

        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }

    public class SaleOrderDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        [Display(Name = "Product")]
        public Guid? ProductID { get; set; }
        [Display(Name = "Product Model")]
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
        [Display(Name = "CGST")]
        public decimal? CGSTAmt { get; set; }
        [Display(Name = "SGST")]
        public decimal? SGSTAmt { get; set; }
        [Display(Name = "IGST")]
        public decimal? IGSTAmt { get; set; }
        [Display(Name = "Cess Percentage")]
        public decimal? CessPerc { get; set; }
        [Display(Name = "Cess Amount")]
        public decimal? CessAmt { get; set; }


        //Additional Fields        
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public TaxTypeViewModel TaxType { get; set; }
    }

}