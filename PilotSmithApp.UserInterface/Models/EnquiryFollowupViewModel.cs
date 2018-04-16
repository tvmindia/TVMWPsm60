using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class EnquiryFollowupViewModel
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public DateTime FollowupDate { get; set; }
        public DateTime FollowupTime { get; set; }
        public int PriorityCode { get; set; }
        [Required(ErrorMessage = "Subject is missing")]
        public string Subject { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public int? RemindPriorTo { get; set; }
        public string ReminderType { get; set; }
        [Required(ErrorMessage = "Status is missing")]
        public string Status { get; set; }
        public string GeneralNotes { get; set; }
        //Additional fields
        [Display(Name ="Followup Date")]
        [Required(ErrorMessage = "Followup Date is missing")]
        public string FollowupDateFormatted { get; set; }
        [Display(Name ="Followup Time")]
        [Required(ErrorMessage = "Followup Time is missing")]
        public string FollowupTimeFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<EnquiryFollowupViewModel> EnquiryFollowupList { get; set;}
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}