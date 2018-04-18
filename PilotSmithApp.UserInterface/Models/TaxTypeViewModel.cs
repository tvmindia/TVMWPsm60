using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class TaxTypeViewModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public List<SelectListItem> TaxTypeSelectList { get; set; }
    }
}