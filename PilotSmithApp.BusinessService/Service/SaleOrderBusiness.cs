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
    public class SaleOrderBusiness:ISaleOrderBusiness
    {
        ISaleOrderRepository _saleOrderRepository;
        public SaleOrderBusiness(ISaleOrderRepository saleOrderRepository)
        {
            _saleOrderRepository = saleOrderRepository;
        }
        public List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm)
        {
            return _saleOrderRepository.GetSaleOrderForSelectListOnDemand(searchTerm);
        }

    }
}
