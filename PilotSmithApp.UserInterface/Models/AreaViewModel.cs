using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class AreaViewModel
    {
        public int Code { get; set; }
        [Display(Name ="State")]
        public int StateCode { get; set; }
        [Display(Name = "District")]
        public int DistrictCode { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> AreaSelectList { get; set; }
        public StateViewModel State { get; set; }
        public DistrictViewModel District { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }

    public class AreaAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}