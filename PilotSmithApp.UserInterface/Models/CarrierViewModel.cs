using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CarrierViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> CarrierSelectList { get; set; }
    }
}