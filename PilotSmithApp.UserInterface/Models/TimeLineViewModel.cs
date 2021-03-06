﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class TimeLineViewModel
    {
        public String DocumentType { get; set; }
        public Guid DocumentID { get; set; }
        public String DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDateFormatted { get; set; }
        public string URL { get; set; }
        public string Particulars { get; set; }
    }
    public class TimeLineViewModelList {
        public List<TimeLineViewModel> TimeLineList { get; set; }
        public String CurrentDocument { get; set; }
    }
}