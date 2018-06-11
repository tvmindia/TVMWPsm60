using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CountryViewModel
    {
        public int Code { get; set; }
        [Display(Name ="Country")]
        [Remote(action: "CheckCountryExist", controller: "Country", AdditionalFields = "IsUpdate,Code")]
        [Required(ErrorMessage = "Country Name is missing")]
        public string Description { get; set; }
        //additional fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> CountrySelectList { get; set; }
    }

    public class CountryAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}