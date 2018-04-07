using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }
        [Required]
        [Remote(action: "CheckProductCodeExist", controller: "Product", AdditionalFields = "IsUpdate,ID")]
        public string Code { get; set; }
        public string Name { get; set; }
        [Display(Name="Category")]
        public int ProductCategoryCode { get; set; }
        public DateTime IntroducedDate { get; set; } 
        [Display(Name ="Company")]       
        public Guid CompanyID { get; set; }
        public string HSNCode { get; set; }

        //Additional Fields
        [Display(Name = "Introduced Date")]
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> ProductSelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public CompanyViewModel Company { get; set; }
    }

    public class ProductAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}