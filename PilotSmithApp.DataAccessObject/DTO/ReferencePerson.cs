using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ReferencePerson
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int? ReferenceTypeCode { get; set; }
        public int? AreaCode { get; set; }
        public string Organization { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNos { get; set; }
        public string FaxNos { get; set; }
        public string GeneralNotes { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public ReferenceType ReferenceType { get;set;}
        public Area Area { get; set; }

    }
    public class ReferencePersonAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public int? AdvReferenceTypeCode { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? AdvAreaCode { get; set; }
        public Area Area { get; set; }
    }
}
