using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class EnquiryFollowup
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public DateTime FollowupDate { get; set; }
        public DateTime FollowupTime { get; set; }
        public int PriorityCode { get; set; }
        public string Subject { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public int? RemindPriorTo { get; set; }
        public string ReminderType { get; set; }
        public string Status { get; set; }
        public string GeneralNotes { get; set; }
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public string FollowupDateFormatted { get; set; }
        public string FollowupTimeFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
