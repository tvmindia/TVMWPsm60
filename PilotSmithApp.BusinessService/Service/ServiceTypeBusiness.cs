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
    public class ServiceTypeBusiness: IServiceTypeBusiness
    {
        IServiceTypeRepository _serviceTypeRepository;
        public ServiceTypeBusiness(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        public List<SelectListItem> GetServiceTypeSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<ServiceType> serviceTypeList = _serviceTypeRepository.GetServiceTypeSelectList();
            return selectListItem = serviceTypeList != null ? (from serviceType in serviceTypeList
                                                                  select new SelectListItem
                                                                  {
                                                                      Text = serviceType.Name,
                                                                      Value = serviceType.Code.ToString(),
                                                                      Selected = false
                                                                  }).ToList() : new List<SelectListItem>();
        }
    }
}
