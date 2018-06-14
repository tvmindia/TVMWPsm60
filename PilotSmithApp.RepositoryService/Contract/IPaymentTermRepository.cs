using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IPaymentTermRepository
    {
        object InsertUpdatePaymentTerm(PaymentTerm paymentTerm);
        List<PaymentTerm> GetAllPayTerm(PaymentTermAdvanceSearch paymentTermAdvanceSearch);
        PaymentTerm GetPaymentTerm(string code);
        bool CheckPaymentTermNoOfDaysExist(PaymentTerm paymentTerm);
        object DeletePaymentTerm(string code);
        List<PaymentTerm> GetPaymentTermForSelectList();

    }
}
