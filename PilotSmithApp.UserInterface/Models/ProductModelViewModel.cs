using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductModelViewModel
    {
        public Guid ID { get; set; }
        [Display(Name ="Product")]
        [Required(ErrorMessage ="Product is missing")]
        public Guid? ProductID { get; set; }
        [Remote(action: "CheckProductModelNameExist", controller: "ProductModel", AdditionalFields = "IsUpdate,ID")]
        [Required(ErrorMessage = "Product name is missing")]
        public string Name { get; set; }
        [Display(Name="Unit")]
        [Required(ErrorMessage = "Unit is missing")]
        public int? UnitCode { get; set; }
        public string Specification { get; set; }
        [Display(Name = "Cost Price")]
        public decimal? CostPrice { get; set; }
        [Display(Name = "Selling Price")]
        public decimal? SellingPrice { get; set; }
        public DateTime IntroducedDate { get; set; }
        [Display(Name = "Stock Quantity")]
        public decimal? StockQty { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }

        //Additional Fields
        [Display(Name = "Introduced Date")]
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> ProductModelSelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public int? ProductSpecificationCode { get; set; }
        public ProductViewModel Product { get; set; }
        public UnitViewModel Unit { get; set; }
       // public Guid hdnFileID { get; set; }
    }
    public class ProductModelAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}