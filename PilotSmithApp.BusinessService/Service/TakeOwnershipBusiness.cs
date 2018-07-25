using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace PilotSmithApp.BusinessService.Service
{
    public class TakeOwnershipBusiness : ITakeOwnershipBusiness
    {
        private ITakeOwnershipRepository _takeOwnershipRepository;
        private IMailBusiness _mailBusiness;
        public TakeOwnershipBusiness(ITakeOwnershipRepository takeOwnershipRepository, IMailBusiness mailBusiness)
        {
            _takeOwnershipRepository = takeOwnershipRepository;
            _mailBusiness = mailBusiness;
        }

        public DocumentLog InsertTakeOwnership(DocumentLog documentLog)
        {
            try
            {
                DocumentLog documentLogOut = _takeOwnershipRepository.InsertTakeOwnership(documentLog);
                bool sendsuccess = false;
                string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/TakeOwnershipAcknowledgement.html"));
                MailMessage _mail = new MailMessage();
                string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                _mail.Body = mailBody.Replace("$DocumentOwner$", documentLogOut.OldUserName).Replace("$Document$", "Enquiry").Replace("$DocumentNo$", documentLogOut.DocumentNo).Replace("$DocumentDate$", documentLog.DateFormatted).Replace("$Logo$", link);
                _mail.To.Add(documentLogOut.OldUserEmail);
                _mail.Subject = "Ownership Change";
                _mail.IsBodyHtml = true;
                 sendsuccess = _mailBusiness.MailMessageSend(_mail);
                return documentLogOut;
            }
            catch( Exception ex)
            {
                throw ex;
            }
        }

    }
}
