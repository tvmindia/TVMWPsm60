using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class PaymentTermBusiness:IPaymentTermBusiness
    {
        private IPaymentTermRepository _paymentTermRepository;

        public PaymentTermBusiness(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }
        public object InsertUpdatePaymentTerm(PaymentTerm paymentTerm)
        {
            return _paymentTermRepository.InsertUpdatePaymentTerm(paymentTerm);
        }
        public List<PaymentTerm> GetAllPayTerm(PaymentTermAdvanceSearch paymentTermAdvanceSearch)
        {
            return _paymentTermRepository.GetAllPayTerm(paymentTermAdvanceSearch);
        }
        public PaymentTerm GetPaymentTerm(string code)
        {
            return _paymentTermRepository.GetPaymentTerm(code);
        }
        public bool CheckPaymentTermNoOfDaysExist(PaymentTerm paymentTerm)
        {
            return _paymentTermRepository.CheckPaymentTermNoOfDaysExist(paymentTerm);
        }
        public object DeletePaymentTerm(string code)
        {
            return _paymentTermRepository.DeletePaymentTerm(code);
        }
        public List<SelectListItem> GetPaymentTermForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<PaymentTerm> PayTermList = _paymentTermRepository.GetPaymentTermForSelectList();
            return selectListItem = PayTermList!=null?(from paymentTerm in PayTermList
                                     select new SelectListItem
                                     {
                                         Text = paymentTerm.Description,
                                         Value = paymentTerm.Code,
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
        
    }
}
