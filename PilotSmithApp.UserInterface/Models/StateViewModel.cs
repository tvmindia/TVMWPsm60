using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class StateViewModel
    {      
        public int Code { get; set; }
        [Required(ErrorMessage ="Description is missing")]
        [Remote(action: "CheckStateNameExist", controller: "State", AdditionalFields = "IsUpdate,Code")]
        public string Description { get; set; }

        //Additional Fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> StateSelectList { get; set; }
        [Display(Name ="Country")]
        //[Required(ErrorMessage ="Country is mising")]
        public int? CountryCode { get; set; }
        public CountryViewModel Country { get; set; } 
    }

    public class StateAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}