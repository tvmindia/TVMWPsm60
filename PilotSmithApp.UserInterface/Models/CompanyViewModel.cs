﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CompanyViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        //Additional Fields
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class CompanyAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}