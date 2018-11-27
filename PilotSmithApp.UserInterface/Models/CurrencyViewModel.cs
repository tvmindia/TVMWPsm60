using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CurrencyViewModel
    {
        [Required(ErrorMessage ="Currency code is missing")]
        [Display(Name ="Select currency code")]
        public string Code { get; set; }
        public string Description { get; set; }
        public string DisplayInWords { get; set; }
        
        //Additional Fields
        public decimal SelectedCurrency { get; set; }
        public decimal CurrencyRate { get; set; }
        public string DocumentType { get; set; }
        public Guid DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public bool IsUpdate { get; set; }
      //  public List<SelectListItem> CurrencySelectList { get; set; }
        public List<CurrencyViewModel> CurrencyList { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}