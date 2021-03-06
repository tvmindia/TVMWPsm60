﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IDocumentApprovalBusiness
    {
        List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode);
        List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch);
        DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode);
        
        object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode,DateTime approvalDate, string Remarks);
        object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode,string Remarks, DateTime rejectionDate);
        object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode);
        List<DocumentApprover> GetApproversByDocType(string docTypeCode);
        object SendDocForApproval(Guid documentID, string documentTypeCode, string approvers, string createdBy, DateTime createdDate);
        object ReSendDocForApproval(Guid documentID, string documentTypeCode, Guid latestApprovalID, string createdBy, DateTime createdDate);
        Task<bool> SendApprolMails(Guid documentID, string documentType);
        List<DocumentApproval> GetStockAdjApprovalSummary();
        List<DocumentApproval> GetAllApprovalHistory(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch);
        object RecallDocument(Guid documentID, string documentTypeCode,string documentNo, DateTime recallDate, string createdBy, DateTime createdDate);
        Task<bool> SendRecallMails(Guid documentID,string docuementTypeCode);
    }
}
