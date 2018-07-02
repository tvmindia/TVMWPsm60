using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProformaInvoiceBusiness
    {
        List<ProformaInvoice> GetAllProformaInvoice(ProformaInvoiceAdvanceSearch proformaInvoiceAdvanceSearch);
        ProformaInvoice GetProformaInvoice(Guid id);
        List<ProformaInvoiceDetail> GetProformaInvoiceDetailListByProformaInvoiceID(Guid proformaInvoiceID);
        object InsertUpdateProformaInvoice(ProformaInvoice proformaInvoice);
        object DeleteProformaInvoice(Guid id);
        object DeleteProformaInvoiceDetail(Guid id);
        object DeleteProformaInvoiceOtherChargeDetail(Guid id);
        Task<bool> ProformaInvoiceEmailPush(ProformaInvoice proformaInvoice);
        List<ProformaInvoiceOtherCharge> GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(Guid proformaInvoiceID);
        object UpdateProformaInvoiceEmailInfo(ProformaInvoice proformaInvoice);
        List<ProformaInvoice> GetProformaInvoiceForSelectListOnDemand(string searchTerm);
        List<SelectListItem> GetProformaInvoiceForSelectList(Guid? proformaInvoiceID);
    }
}
