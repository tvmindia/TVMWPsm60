using PilotSmithApp.BusinessService.Contracts;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class DynamicUIBusiness:IDynamicUIBusiness
    {
        private IDynamicUIRepository _dynamicUIRepository;
        public DynamicUIBusiness(IDynamicUIRepository dynamicUIRespository)
        {
            _dynamicUIRepository = dynamicUIRespository;

        }
        public List<PSASysMenu> GetAllMenues()
        {
            return _dynamicUIRepository.GetAllMenues();
        }
    }
}
