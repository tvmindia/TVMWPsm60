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
    public class DeliveryChallanBusiness:IDeliveryChallanBusiness
    {
        private IDeliveryChallanRepository _deliveryChallanRepository;
        ICommonBusiness _commonBusiness;
        public DeliveryChallanBusiness(IDeliveryChallanRepository deliveryChallanRepository,ICommonBusiness commonBusiness)
        {
            _deliveryChallanRepository = deliveryChallanRepository;
            _commonBusiness = commonBusiness;
        }
        public List<DeliveryChallan> GetAllDeliveryChallan(DeliveryChallanAdvanceSearch deliveryChallanAdvanceSearch)
        {
            return _deliveryChallanRepository.GetAllDeliveryChallan(deliveryChallanAdvanceSearch);
        }
        public DeliveryChallan GetDeliveryChallan (Guid id)
        {
            return _deliveryChallanRepository.GetDeliveryChallan(id);
        }
        public List<DeliveryChallanDetail> GetDeliveryChallanDetailListByDeliveryChallanID(Guid deliveryChallanID)
        {
            return _deliveryChallanRepository.GetDeliveryChallanDetailListByDeliveryChallanID(deliveryChallanID);
        }
        public object InsertUpdateDeliveryChallan(DeliveryChallan deliveryChallan)
        {
            if (deliveryChallan.DeliveryChallanDetailList.Count > 0)
            {
                deliveryChallan.DetailXML = _commonBusiness.GetXMLfromDeliveryChallanObject(deliveryChallan.DeliveryChallanDetailList, "ProductID, ProductModelID, ProductSpec, UnitCode");
            }
            return _deliveryChallanRepository.InsertUpdateDeliveryChallan(deliveryChallan);
        }
        public object DeleteDeliveryChallan(Guid id)
        {
            return _deliveryChallanRepository.DeleteDeliveryChallan(id);
        }
        public object DeleteDeliveryChallanDetail(Guid id)
        {
            return _deliveryChallanRepository.DeleteDeliveryChallanDetail(id);
        }
        public List<DeliveryChallan> GetDeliveryChallanForSelectListOnDemand(string searchTerm)
        {
            return _deliveryChallanRepository.GetDeliveryChallanForSelectListOnDemand(searchTerm);
        }
        public List<SelectListItem> GetDeliveryChallanForSelectList(Guid? id)
        {
            List<SelectListItem> selectListItem = null;
            List<DeliveryChallan> deliveryChallanList = _deliveryChallanRepository.GetDeliveryChallanForSelectList(id);
            return selectListItem = deliveryChallanList != null ? (from deliveryChallan in deliveryChallanList
                                                                   select new SelectListItem
                                                                   {
                                                                       Text = deliveryChallan.DelvChallanNo,
                                                                       Value = deliveryChallan.ID.ToString(),
                                                                       Selected = false
                                                                   }).ToList() : new List<SelectListItem>();
        }
    }
}
