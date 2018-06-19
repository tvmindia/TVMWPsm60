﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class DocumentApprovalViewModel
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
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<DocumentApprovalViewModel> DocumentApprovalList { get; set; }
    }

    public class ApprovalHistoryViewModel
    {
        public Guid ApproverID { get; set; }
        public string ApproverName { get; set; }
        public string ApproverLevel { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }

    }
    public class DocumentSummaryViewModel
    {
        public DataTable DataTable { get; set; }
        public Guid DocumentID { get; set; }
        public string DocumentTypeCode { get; set; }
    }

    public class DocumentApprovalAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Document Date From")]
        public string FromDate { get; set; }
        [Display(Name = "Document Date To")]
        public string ToDate { get; set; }
        [Display(Name = "Document Type")]
        public string DocumentTypeCode { get; set; }
        public DocumentTypeViewModel DocumentType { get; set; }
        [Display(Name = "Approval Status")]
        public int? ApprovalStatus { get; set; }
        [Display(Name = "Approval Level")]
        public int? ApproverLevel { get; set; }

        [Display(Name = "Show All Level Documents")]
        public Boolean ShowAll { get; set; }
        public String LoginName { get; set; }
    }

    public class DocumentApproverViewModel
    {
        public Guid? ApproverID { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public int ApproverLevel { get; set; }
        public string ApproverCSV { get; set; }
        public bool IsDefault { get; set; }
        public int ApproversCount { get; set; }
        public List<DocumentApproverViewModel> SendForApprovalList { get; set; }
    }

}