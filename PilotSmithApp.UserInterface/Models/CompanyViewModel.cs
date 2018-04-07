using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CompanyViewModel
    {
        public Guid ID { get; set; }
        [Required (ErrorMessage ="Company Name is Missing")]
        public string Name { get; set; }

        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> CompanySelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class CompanyAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}