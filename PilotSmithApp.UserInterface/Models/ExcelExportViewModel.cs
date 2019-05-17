using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ExcelExportViewModel
    {
        public string AdvanceSearch { get; set; }
        public string DocumentType { get; set; }
        public string[] TableHeaderColumns { get; set; }
        public string[] TableHeaderColumnsWidth { get; set; }
        public string OptionValue { get; set; }
    }
}