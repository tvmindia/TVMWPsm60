using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class PlantViewModel
    {
        public int Code { get; set; }
        [Remote(action: "CheckPlantExist", controller: "Plant", AdditionalFields = "IsUpdate,Code")]
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        //additional fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> PlantSelectList { get; set; }
    }

    public class PlantAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}