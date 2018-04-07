using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class PaymentTermViewModel
    {
        [Required(ErrorMessage = "Payment Term is missing")]
        [Display(Name = "Payment Term")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        public int? NoOfDays { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> PaymentTermSelectList { set; get; }
    }
}