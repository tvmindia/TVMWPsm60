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
    public class SalesInvoieBusiness: ISalesInvoieBusiness
    {
        #region Constructor Injection
        ISalesInvoiceRepository _salesInvoiceRepository;
        ICommonBusiness _commonBusiness;
        public SalesInvoieBusiness(ISalesInvoiceRepository salesInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        public List<SalesSummary> GetSalesSummary()
        {
            return _salesInvoiceRepository.GetSalesSummary();
        }
    }
}
