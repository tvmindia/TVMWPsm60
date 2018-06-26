
using System;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Approver
    {
        public Guid ID { get; set; }
        public string DocumentTypeCode { get; set; }
        public int Level { get; set; }
        public Guid UserID { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public PSAUser PSAUser { get; set; }
        public DocumentType DocumentType { get; set; }
    }

    public class ApproverAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string AdvDocumentTypeCode { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public DocumentType DocumentType { get; set; }
        public Approver Approver { get; set; }
    }
}
