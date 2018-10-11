﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class DocumentApproval
    {
            public string DocumentTypeCode { get; set; }
            public string DocumentType { get; set; }
            public string DocumentNo { get; set; }
            public DateTime DocumentDate { get; set; }
            public string DocumentDateFormatted { get; set; }
            public int StatusCode { get; set; }
            public string DocumentStatus { get; set; }
            public int ApproverLevel { get; set; }
            public string Approver { get; set; }
            public string DocumentCreatedBy { get; set; }
            public Guid ApprovalLogID { get; set; }
            public Guid DocumentID { get; set; }
            public Guid UserID { get; set; }
            public Guid ApproverID { get; set; }
            public Guid LastApprovedUserID { get; set; }
            public string LatestDocumentStatus { get; set; }
            public Boolean IsNextApprover { get; set; }
            public string DocumentOwner { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<DocumentApproval> DocumentApprovalList { get; set; }
    }
    public class ApprovalHistory
    {
        public Guid ApproverID { get; set; }
        public string ApproverName { get; set; }
        public string ApproverLevel { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
    }
    public class DocumentSummary
    {
        public object DataTable { get; set; }
        public Guid DocumentID { get; set; }
        public string DocumentTypeCode { get; set; }
    }

    public class DocumentApprovalAdvanceSearch
    {
       
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }     
        public string FromDate { get; set; }      
        public string ToDate { get; set; }       
        public DocumentType DocumentType { get; set; }
        public Boolean ShowAll { get; set; } 
        public String LoginName { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApproverLevel { get; set; }
    }

    public class DocumentApprover
    {
        public Guid? ApproverID { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public int ApproverLevel { get; set; }
        public string ApproverCSV { get; set; }
        public bool IsDefault { get; set; }

    }

    public class DocumentApprovalMailDetail
    {
      
        public string Status { get; set; }
        public string NextApprover { get; set; }
        public string NextApproverEmail { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string DocumentOwner { get; set; }
        public string DocumnetOwnerMail { get; set; }
        public Guid ApprovalID { get; set; }
        public string Remarks { get; set; }

    }

    public class DocumentRecallMailDetail
    {
        public string Approver { get; set; }
        public string ApproverEmail { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string DocumentOwner { get; set; }
        public string DocumnetOwnerMail { get; set; }
        public DateTime RecallDate { get; set; }       
        public Guid ApprovalID { get; set; }
    }
}
