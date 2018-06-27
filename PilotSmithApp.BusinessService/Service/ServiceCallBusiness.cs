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
    public class ServiceCallBusiness : IServiceCallBusiness
    {
        IServiceCallRepository _ServiceCallRepository;
        ICommonBusiness _commonBusiness;
        ITaxTypeBusiness _taxTypeBusiness;
        IMailBusiness _mailBusiness;
        public ServiceCallBusiness(IServiceCallRepository ServiceCallRepository, ICommonBusiness commonBusiness, ITaxTypeBusiness taxTypeBusiness, IMailBusiness mailBusiness)
        {
            _ServiceCallRepository = ServiceCallRepository;
            _commonBusiness = commonBusiness;
            _taxTypeBusiness = taxTypeBusiness;
            _mailBusiness = mailBusiness;
        }

        public List<ServiceCall> GetAllServiceCall(ServiceCallAdvanceSearch serviceCallAdvanceSearch)
        {
            return _ServiceCallRepository.GetAllServiceCall(serviceCallAdvanceSearch);
        }

        public List<ServiceCallDetail> GetServiceCallDetailListByServiceCallID(Guid quoteID)
        {
            return _ServiceCallRepository.GetServiceCallDetailListByServiceCallID(quoteID);
        }

        public List<ServiceCallCharge> GetServiceCallChargeDetailListByServiceCallID(Guid serviceCallID)
        {
            return _ServiceCallRepository.GetServiceCallChargeDetailListByServiceCallID(serviceCallID);
        }

        public ServiceCall GetServiceCall(Guid id)
        {
            return _ServiceCallRepository.GetServiceCall(id);
        }

        public object InsertUpdateServiceCall(ServiceCall serviceCall)
        {
            if (serviceCall.ServiceCallDetailList.Count > 0)
            {
                serviceCall.DetailXML = _commonBusiness.GetXMLfromServiceCallObject(serviceCall.ServiceCallDetailList, "ProductID, ProductModelID, InstalledDateFormatted");
            }
            if (serviceCall.ServiceCallChargeList.Count > 0)
            {
                serviceCall.CallChargeXML = _commonBusiness.GetXMLfromServiceCallChargeObject(serviceCall.ServiceCallChargeList, "OtherChargeCode , ChargeAmount");
            }
            return _ServiceCallRepository.InsertUpdateServiceCall(serviceCall);
        }

        public object DeleteServiceCall(Guid id)
        {
            return _ServiceCallRepository.DeleteServiceCall(id);
        }

        public object DeleteServiceCallDetail(Guid id)
        {
            return _ServiceCallRepository.DeleteServiceCallDetail(id);
        }

        public object DeleteServiceCallChargeDetail(Guid id)
        {
            return _ServiceCallRepository.DeleteServiceCallChargeDetail(id);
        }

        //public ServiceCallCharge CalculateGST(ServiceCallCharge serviceCallCharge)
        //{
        //    TaxType taxType = _taxTypeBusiness.GetTaxType((int)serviceCallCharge.TaxTypeCode);
        //    serviceCallCharge.CGSTPerc = ((serviceCallCharge.ChargeAmount) * (taxType.CGSTPercentage)) / 100;
        //    serviceCallCharge.SGSTPerc = ((serviceCallCharge.ChargeAmount) * (taxType.SGSTPercentage)) / 100;
        //    serviceCallCharge.IGSTPerc = ((serviceCallCharge.ChargeAmount) * (taxType.IGSTPercentage)) / 100;
        //    return serviceCallCharge;
        //}
    }
}
