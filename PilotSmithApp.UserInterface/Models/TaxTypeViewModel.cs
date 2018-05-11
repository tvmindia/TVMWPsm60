using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class TaxTypeViewModel
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        [Remote(action: "CheckTaxTypeNameExist", controller: "TaxType", AdditionalFields = "IsUpdate,Code")]
        public string Description { get; set; }
        [Display(Name = "CGST Percentage")]
        public decimal CGSTPercentage { get; set; }
        [Display(Name = "SGST Percentage")]
        public decimal SGSTPercentage { get; set; }
        [Display(Name = "IGST Percentage")]
        public decimal IGSTPercentage { get; set; }
        public string ValueText { get; set; }
        public List<SelectListItem> TaxTypeSelectList { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class TaxTypeAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}