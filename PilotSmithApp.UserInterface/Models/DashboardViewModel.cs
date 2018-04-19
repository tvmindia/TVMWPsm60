using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class DashboardViewModel
    {
    }
    public class SalesSummaryList
    {
        public List<SalesSummaryViewModel> SalesSummaryVMList { get; set; }
    }

    public class EnquiryFollowupSummaryList
    {
        public List<EnquiryValueFolloupSummaryViewModel> EnquiryFollowupSummaryVMList { get; set; }
    }
}