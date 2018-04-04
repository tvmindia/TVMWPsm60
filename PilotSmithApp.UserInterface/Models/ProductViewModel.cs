﻿using System;
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
        public string Code { get; set; }
        public string Name { get; set; }
        [Display(Name="Category Code")]
        public int CategoryCode { get; set; }
        public DateTime IntroducedDate { get; set; }        
        public Guid CompanyID { get; set; }
        public string HSNCode { get; set; }

        //Additional Fields
        [Display(Name = "Introduced Date")]
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class ProductAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}