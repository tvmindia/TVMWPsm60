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
        public List<SelectListItem> GetSaleOrderForSelectList(Guid? id)
        {
            List<SelectListItem> selectListItem = null;
            List<SaleOrder> saleOrderList = _saleOrderRepository.GetSaleOrderForSelectList(id);
            return selectListItem = saleOrderList != null ? (from saleOrder in saleOrderList
                                                             select new SelectListItem
                                                             {
                                                                 Text = saleOrder.SaleOrderNo,
                                                                 Value = saleOrder.ID.ToString(),
                                                                 Selected = false
                                                             }).ToList() : new List<SelectListItem>();
        }

    }
}
