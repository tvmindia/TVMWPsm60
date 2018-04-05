using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class BranchViewModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public PSASysCommonViewModel psaSysCommon { get; set; }
        public List<SelectListItem> BranchList { get; set; }
    }
}