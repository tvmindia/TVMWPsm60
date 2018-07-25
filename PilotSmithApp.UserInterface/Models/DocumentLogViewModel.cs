using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class DocumentLogViewModel
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}