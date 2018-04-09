using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ProductSpecificationViewModel
    {
        [Required]
        public int Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        [Remote(action: "CheckProductSpecificationExist", controller: "ProductSpecification", AdditionalFields = "IsUpdate,Code")]
        public string Description { get; set; }
        //additional fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int ProductSpecificationCode { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> ProductSpecificationSelectList { get; set; }
    }

    public class ProductSpecificationAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}