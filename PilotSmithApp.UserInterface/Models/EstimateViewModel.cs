using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class EstimateViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Estimate No.")]
        public string EstimateNo { get; set; }
        [Display(Name = "Estimate Ref No.")]
        [Required(ErrorMessage = "Estimate Reference No. is missing")]
        public string EstimateRefNo { get; set; }
        public DateTime EstimateDate { get; set; }
        [Display(Name = "Enquiry")]
        public Guid? EnquiryID { get; set; }
        [Display(Name = "Customer")]
        public Guid? CustomerID { get; set; }
        [Display(Name = "Document Status")]
        public int? DocumentStatusCode { get; set; }
        [Display(Name = "Valid upto")]
        public DateTime ValidUpToDate { get; set; }
        [Display(Name = "Prepared By")]
        public Guid? PreparedBy { get; set; }
        [Display(Name = "General notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Document Owner")]
        public Guid? DocumentOwnerID { get; set; }
        [Display(Name = "Branch Code")]
        public int? BranchCode { get; set; }

        //Aditional Fields
        public string DetailJSON { get; set; }
        [Display(Name = "Estimate Date")]
        [Required(ErrorMessage = "Estimate Date is missing")]
        public string EstimateDateFormatted { get; set; }
        public string ValidUpToDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }

    }

    public class EstimateAdvanceSearchViewModel
    {
        public string EstimateDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
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
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? CostRate { get; set; }
        public decimal? SellingRate { get; set; }
        public string DrawingNo { get; set; }
        
        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}