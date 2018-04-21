using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CustomerCategoryViewModel
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        [Remote(action: "CheckCustomerCategoryExist", controller: "CustomerCategory", AdditionalFields = "IsUpdate,Code")]
        public string Name { get; set; }

        //additional fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> CustomerCategorySelectList { get; set; }
    }

    public class CustomerCategoryAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}