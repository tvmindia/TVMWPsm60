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
        [Display(Name ="Followup Date")]
        public DateTime FollowupDate { get; set; }
        [Display(Name = "Followup Time")]
        public DateTime FollowupTime { get; set; }
        [Required(ErrorMessage ="Priority is required")]
        [Display(Name = "Priority")]
        public int PriorityCode { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        [Display(Name = "Remind Prior To")]
        public int? RemindPriorTo { get; set; }
        [Display(Name = "Reminder Type")]
        public string ReminderType { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        //Additional fields
        [Display(Name ="Followup Date")]
        [Required(ErrorMessage = "Followup Date is required")]
        public string FollowupDateFormatted { get; set; }
        [Display(Name ="Followup Time")]
        [Required(ErrorMessage = "Followup Time is required")]
        public string FollowupTimeFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public CustomerViewModel Customer { get; set; }
        public List<EnquiryFollowupViewModel> EnquiryFollowupList { get; set;}
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}