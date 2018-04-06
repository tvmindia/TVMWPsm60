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
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string RequirementSpec { get; set; }
        public Guid CustomerID { get; set; }
        public int GradeCode { get; set; }
        public int StatusCode { get; set; }
        public int ReferredByCode { get; set; }
        public Guid ResponsiblePersonID { get; set; }
        public Guid AttendedByID { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        [Required(ErrorMessage ="Branch Code is missing")]
        public int BranchCode { get; set; }
        //Additional properties
        [Display(Name ="Enquiry Date")]
        public string EnquiryDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public CustomerViewModel Customer { get; set; }
        public BranchViewModel Branch { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
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
        public Guid ProductID { get; set; }
        public Guid ModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal Qty { get; set; }
        public int UnitCode { get; set; }
        public decimal Rate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}