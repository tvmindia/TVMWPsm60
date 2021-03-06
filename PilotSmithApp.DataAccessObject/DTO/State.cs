﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class State
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public int? CountryCode { get; set; }
        public Country Country { get; set; }
    }

    public class StateAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
