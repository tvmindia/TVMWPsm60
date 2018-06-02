using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace PilotSmithApp.UserInterface.Models
{
    public class ApprovalStatusViewModel
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        public List<SelectListItem> ApprovalStatusSelectList { get; set; }
    }
}