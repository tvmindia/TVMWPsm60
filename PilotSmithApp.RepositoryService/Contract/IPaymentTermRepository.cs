﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IPaymentTermRepository
    {
        List<PaymentTerm> GetAllPayTerm();
        List<PaymentTerm> GetPaymentTermForSelectList();

    }
}
