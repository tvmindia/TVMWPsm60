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
        public string Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        [Display(Name = "No Of Days")]
        [Required(ErrorMessage = "No Of Days is missing")]
        [Remote(action: "CheckPaymentTermNoOfDaysExist", controller: "PaymentTerm", AdditionalFields = "Code")]
        public int? NoOfDays { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> PaymentTermSelectList { set; get; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class PaymentTermAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}