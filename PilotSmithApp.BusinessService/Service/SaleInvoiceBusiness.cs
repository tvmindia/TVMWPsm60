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
    public class SaleInvoiceBusiness: ISaleInvoiceBusiness
    {
        ISaleInvoiceRepository _saleInvoiceRepository;
        ICommonBusiness _commonBusiness;
        public SaleInvoiceBusiness(ISaleInvoiceRepository saleInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _saleInvoiceRepository = saleInvoiceRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SaleInvoice> GetAllSaleInvoice(SaleInvoiceAdvanceSearch saleInvoiceAdvanceSearch)
        {
            return _saleInvoiceRepository.GetAllSaleInvoice(saleInvoiceAdvanceSearch);
        }
        public List<SaleInvoiceDetail> GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceID);
        }
        public object InsertUpdateSaleInvoice(SaleInvoice saleInvoice)
        {
            if (saleInvoice.SaleInvoiceDetailList.Count > 0)
            {
                saleInvoice.DetailXML = _commonBusiness.GetXMLfromSaleInvoiceObject(saleInvoice.SaleInvoiceDetailList, "ProductID");
            }
            return _saleInvoiceRepository.InsertUpdateSaleInvoice(saleInvoice);
        }
        public SaleInvoice GetSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.GetSaleInvoice(id);
        }
        public object DeleteSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoice(id);
        }
        public object DeleteSaleInvoiceDetail(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoiceDetail(id);
        }
        public List<SaleInvoiceOtherCharge> GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceID);
        }
    }
}
