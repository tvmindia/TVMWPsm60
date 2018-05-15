using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class OtherCharge
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string SACCode { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class OtherChargeAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
