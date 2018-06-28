using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class BankViewModel
    {
        public int Code { get; set; }
        [Remote(action: "CheckBankExist", controller: "Bank", AdditionalFields = "IsUpdate,Code")]
        [Required(ErrorMessage = "Bank is missing")]
        public string Name { get; set; }

        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
    public class BankAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}