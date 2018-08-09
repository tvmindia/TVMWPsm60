using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IMailBusiness
    {
        Task<bool> MailSendAsync(Mail mailObj);
        Task<bool> MailMessageSendAsync(MailMessage mailObj);
        bool MailMessageSend(MailMessage mailObj);
    }
}
