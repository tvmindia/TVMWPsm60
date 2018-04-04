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
        public List<PaymentTerm> GetAllPayTerm()
        {
            return _paymentTermRepository.GetAllPayTerm();
        }
        public List<SelectListItem> GetPaymentTermForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<PaymentTerm> PayTermList = _paymentTermRepository.GetPaymentTermForSelectList();
            if(PayTermList!=null)
            foreach (PaymentTerm PayT in PayTermList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PayT.Description,
                    Value = PayT.Code,
                    Selected = false
                });
            }
            return selectListItem;
        }
        
    }
}
