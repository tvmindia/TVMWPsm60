using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IServiceCallBusiness
    {
        List<ServiceCall> GetAllServiceCall(ServiceCallAdvanceSearch ServiceCallAdvanceSearch);
        List<ServiceCallDetail> GetServiceCallDetailListByServiceCallID(Guid ServiceCallID);
        List<ServiceCallCharge> GetServiceCallChargeDetailListByServiceCallID(Guid ServiceCallID);
        ServiceCall GetServiceCall(Guid id);
        //ServiceCallCharge CalculateGST(ServiceCallCharge serviceCallCharge);
        object InsertUpdateServiceCall(ServiceCall ServiceCall);
        object DeleteServiceCall(Guid id);
        object DeleteServiceCallDetail(Guid id);
        object DeleteServiceCallChargeDetail(Guid id);
    }
}
