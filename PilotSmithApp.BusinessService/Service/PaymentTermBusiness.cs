using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class PaymentTermBusiness:IPaymentTermBusiness
    {
        private IPaymentTermRepository _paymentTermRepository;

        public PaymentTermBusiness(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }
        public List<PaymentTerm> GetAllPayTerm()
        {
            return _paymentTermRepository.GetAllPayTerm();
        }
    }
}
