﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IProformaInvoiceRepository
    {
        List<ProformaInvoice> GetAllProformaInvoice(ProformaInvoiceAdvanceSearch proformaInvoiceAdvanceSearch);
        ProformaInvoice GetProformaInvoice(Guid id);
        List<ProformaInvoiceDetail> GetProformaInvoiceDetailListByProformaInvoiceID(Guid proformaInvoiceID);
        object InsertUpdateProformaInvoice(ProformaInvoice proformaInvoice);
        object DeleteProformaInvoice(Guid id);
        object DeleteProformaInvoiceDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        object DeleteProformaInvoiceOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate);        
        List<ProformaInvoiceOtherCharge> GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(Guid proformaInvoiceID);
        object UpdateProformaInvoiceEmailInfo(ProformaInvoice proformaInvoice);
        List<ProformaInvoice> GetProformaInvoiceForSelectListOnDemand(string searchTerm);
        List<ProformaInvoice> GetProformaInvoiceForSelectList(Guid? proformaInvoiceID);
    }
}
