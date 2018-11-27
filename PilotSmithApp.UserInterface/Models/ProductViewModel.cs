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
        [Required(ErrorMessage ="Product Code is missing")]
        [Remote(action: "CheckProductCodeExist", controller: "Product", AdditionalFields = "IsUpdate,ID")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Product Name is missing")]
        public string Name { get; set; }
        [Display(Name="Category")]
        public int? ProductCategoryCode { get; set; }
        public DateTime? IntroducedDate { get; set; } 
        [Display(Name ="Company")]       
        public Guid? CompanyID { get; set; }
        [Display(Name = "HSN Code")]
        public string HSNCode { get; set; }
        [Display(Name ="Name In Tally")]
        public string TallyName { get; set; }
        [Display(Name="Purpose")]
        public string Purpose { get; set; }

        //Additional Fields
        [Display(Name = "Introduced Date")]
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> ProductSelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnPopupFileID { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public CompanyViewModel Company { get; set; }
        public ProductModelViewModel ProductModel { get; set; } 
    }

    public class ProductAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}