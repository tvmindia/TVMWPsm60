using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.IO;
using PilotSmithApp.BusinessService.Contract;

namespace PilotSmithApp.BusinessService.Service
{
    public class DocumentApprovalBusiness: IDocumentApprovalBusiness
    {
        private IDocumentApprovalRepository _documentApprovalRepository;
        private IMailBusiness _mailBusiness;
        private ICommonBusiness _commonBusiness;
        PSASysCommon _pSASysCommon = new PSASysCommon();

        public DocumentApprovalBusiness(IDocumentApprovalRepository documentApprovalRepository, IMailBusiness mailBusiness, ICommonBusiness commonBusiness)
        {
            _documentApprovalRepository = documentApprovalRepository;
            _mailBusiness = mailBusiness;
            _commonBusiness = commonBusiness;
        }

        public List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode)
        {
          return  _documentApprovalRepository.GetApprovalHistory(DocumentID, DocumentTypeCode);
        }

        public List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch)
        {
            return _documentApprovalRepository.GetAllDocumentsPendingForApprovals(documentApprovalAdvanceSearch);
        }

        public DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.GetDocumentSummary(DocumentID, DocumentTypeCode);
        }

        public object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, DateTime approvalDate, string Remarks)
        {
            return _documentApprovalRepository.ApproveDocument(ApprovalLogID, DocumentID, DocumentTypeCode, approvalDate, Remarks);
        }

        public object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, string Remarks, DateTime rejectionDate)
        {
            return _documentApprovalRepository.RejectDocument(ApprovalLogID, DocumentID, DocumentTypeCode, Remarks, rejectionDate);
        }

        public object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.ValidateDocumentsApprovalPermission(LoginName, DocumentID, DocumentTypeCode);

        }

        public List<DocumentApprover> GetApproversByDocType(string docTypeCode)
        {
            return _documentApprovalRepository.GetApproversByDocType(docTypeCode);
        }

        public object SendDocForApproval(Guid DocumentID, string DocumentTypeCode, string approvers, string CreatedBy, DateTime CreatedDate)
        {
            object result= _documentApprovalRepository.SendDocForApproval(DocumentID, DocumentTypeCode, approvers, CreatedBy, CreatedDate);
          
            return result;
        }

        public object ReSendDocForApproval(Guid documentID, string documentTypeCode, Guid latestApprovalID, string createdBy, DateTime createdDate)
        {
            return _documentApprovalRepository.ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID, createdBy, createdDate);
        }

        public List<DocumentApproval> GetStockAdjApprovalSummary()
        {
            return _documentApprovalRepository.GetStockAdjApprovalSummary();
        }


        public List<DocumentApproval> GetAllApprovalHistory(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch)
        {
            return _documentApprovalRepository.GetAllApprovalHistory(documentApprovalAdvanceSearch);
        }

        public async Task<bool> SendApprolMails(Guid documentID, string documentType) {
            try
            {
                DocumentApprovalMailDetail docAprovalMailDetail = new DocumentApprovalMailDetail();
                bool sendsuccess;
                string link;
                string loginLink= WebConfigurationManager.AppSettings["AppURL"];
                Settings settings = new Settings();
                docAprovalMailDetail = _documentApprovalRepository.GetApprovalMailDetails(documentID, documentType);
                if (docAprovalMailDetail != null) {
                    link= WebConfigurationManager.AppSettings["AppURL"]  + "/DocumentApproval/ViewPendingDocuments?ID=" + docAprovalMailDetail.ApprovalID + "&DocType="+ documentType +"&DocID="+ documentID ;
                    if (docAprovalMailDetail.Status == "PENDING") {

                        string mailBody= File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/SendForApproval.html"));

                        string imgLink = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";

                        Mail mail = new Mail();
                        mail.Body = mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$DocumentOwner$", docAprovalMailDetail.DocumentOwner).Replace("$Name$", docAprovalMailDetail.NextApprover).Replace("$ApproveLink$", link).Replace("$Date$", _pSASysCommon.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$",loginLink).Replace("$Logo$",imgLink);
                        mail.Subject = "Document Pending For Approval (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.NextApproverEmail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                    if (docAprovalMailDetail.Status == "REJECTED")
                    {
                        string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentRejected.html"));
                        string imgLink = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                        Mail mail = new Mail();
                        mail.Body = mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$Name$", docAprovalMailDetail.DocumentOwner).Replace("$Date$", _pSASysCommon.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$", loginLink).Replace("$Approver$", docAprovalMailDetail.NextApprover).Replace("$Remarks$", docAprovalMailDetail.Remarks).Replace("$Logo$", imgLink);
                        mail.Subject = "Document Rejected for ammedment (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.DocumnetOwnerMail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                    if (docAprovalMailDetail.Status == "APPROVED")
                    {
                        string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentApproved.html"));
                        string imgLink = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                        Mail mail = new Mail();
                        mail.Body =  mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$Name$", docAprovalMailDetail.DocumentOwner).Replace("$Date$", _pSASysCommon.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$", loginLink).Replace("$Remarks$", docAprovalMailDetail.Remarks).Replace("$Logo$", imgLink);
                        mail.Subject = "Document Approved (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.DocumnetOwnerMail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                }


                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
