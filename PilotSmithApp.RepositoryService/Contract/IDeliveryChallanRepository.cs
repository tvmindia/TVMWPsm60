using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IDeliveryChallanRepository
    {
        List<DeliveryChallan> GetAllDeliveryChallan(DeliveryChallanAdvanceSearch deliveryChallanAdvanceSearch);
        DeliveryChallan GetDeliveryChallan(Guid id);
        List<DeliveryChallanDetail> GetDeliveryChallanDetailListByDeliveryChallanID(Guid deliveryChallanID);
        object InsertUpdateDeliveryChallan(DeliveryChallan deliveryChallan);
        object DeleteDeliveryChallan(Guid id);
        object DeleteDeliveryChallanDetail(Guid id);
        List<DeliveryChallan> GetDeliveryChallanForSelectListOnDemand(string searchTerm);
        List<DeliveryChallan> GetDeliveryChallanForSelectList(Guid? id);
    }
}
