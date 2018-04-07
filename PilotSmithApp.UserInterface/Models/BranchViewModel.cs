using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class BranchViewModel
    {
        public int Code { get; set; }
        [Required(ErrorMessage ="Description is missing")]
        public string Description { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> BranchList { get; set; }
    }
}