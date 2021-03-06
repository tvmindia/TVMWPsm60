﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IPaymentTermBusiness
    {
        object InsertUpdatePaymentTerm(PaymentTerm paymentTerm);
        List<PaymentTerm> GetAllPayTerm(PaymentTermAdvanceSearch paymentTermAdvanceSearch);
        PaymentTerm GetPaymentTerm(string code);
        bool CheckPaymentTermNoOfDaysExist(PaymentTerm paymentTerm);
        object DeletePaymentTerm(string code);
        List<SelectListItem> GetPaymentTermForSelectList();
    }
}
