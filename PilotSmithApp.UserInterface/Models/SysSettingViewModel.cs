using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class SysSettingViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage ="Name is missing")]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class SysSettingAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}