using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Area
    {
        public int Code { get; set; }
        public int? StateCode { get; set; }
        public int? DistrictCode { get; set; }
        public string Description { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public State State { get; set; }
        public District District { get; set; }
    }

    public class AreaAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
