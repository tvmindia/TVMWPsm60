using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ServiceTypeViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional Fields
        public List<SelectListItem> ServiceTypeSelectList { get; set; }
    }
}