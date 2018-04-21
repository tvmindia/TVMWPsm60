﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ApprovalLogViewModel
    {
        public Guid ID { get; set; }
        public Guid DocumentID { get; set; }
        public Guid ApproverID { get; set; }
        public string DocumentType { get; set; }
        public int ApproverLevel { get; set; }
        public int Status { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsLevelMandatory { get; set; }
        public string Remarks { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}