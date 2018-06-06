using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class BillLocationViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> BillLocationList { get; set; }
    }
}