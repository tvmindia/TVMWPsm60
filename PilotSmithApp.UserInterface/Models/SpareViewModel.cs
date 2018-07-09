using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class SpareViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Spare Code is missing")]
        [Remote(action: "CheckSpareCodeExist", controller: "Spare", AdditionalFields = "IsUpdate,ID")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Spare Name is missing")]
        public string Name { get; set; }
        public string HSNCode { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }

        //Additional Fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnPopupFileID { get; set; }
        public List<SelectListItem> SpareSelectList { get; set; }
    }

    public class SpareAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}