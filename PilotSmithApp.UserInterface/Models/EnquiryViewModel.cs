using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        [Display(Name = "Customer")]
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
        [Display(Name ="Enquiry Date")]
        [Required(ErrorMessage ="Enquiry Date is missing")]
        public string EnquiryDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public EnquiryGradeViewModel EnquiryGrade { get; set; }
        public List<EnquiryDetailViewModel> EnquiryDetailList { get; set; }
    }
    public class EnquiryAdvanceSearchViewModel
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class EnquiryDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}