using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class OtherChargeViewModel
    {
        [Required(ErrorMessage = "Code is missing")]
        public int Code { get; set; }
        [Remote(action: "CheckOtherChargeCodeExist", controller: "OtherCharge", AdditionalFields = "IsUpdate,Code")]
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        [Display(Name = "SAC Code")]
        public string SACCode { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> OtherChargeSelectList { set; get; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class OtherChargeAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}