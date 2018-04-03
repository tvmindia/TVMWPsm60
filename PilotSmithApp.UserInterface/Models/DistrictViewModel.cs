using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class DistrictViewModel
    {
        [Required]
        public int Code { get; set; }
        [Display(Name ="State")]
        public int StateCode { get; set; }
        public string Description { get; set; }

        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int DistrictCode { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public StateViewModel State { get; set; }
    }

    public class DistrictAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}