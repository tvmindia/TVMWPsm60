using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class PSASysCommonViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDatestr { get; set; }
        public string CreatedDateString { get; set; }        
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
    }
}