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
    public class DistrictBusiness:IDistrictBusiness
    {
        private IDistrictRepository _districtRepository;
        public DistrictBusiness(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }
        public object InsertUpdateDistrict(District district)
        {
            return _districtRepository.InsertUpdateDistrict(district);
        }
        public List<District> GetAllDistrict(DistrictAdvanceSearch districtAdvanceSearch)
        {
            return _districtRepository.GetAllDistrict(districtAdvanceSearch);
        }
        public District GetDistrict(int code)
        {
            return _districtRepository.GetDistrict(code);
        }
        public bool CheckDistrictCodeExist(int code)
        {
            return _districtRepository.CheckDistrictCodeExist(code);
        }
        public object DeleteDistrict(int code)
        {
            return _districtRepository.DeleteDistrict(code);
        }
        public List<District> GetDistrictForSelectList()
        {
            return _districtRepository.GetDistrictForSelectList();
        }
    }
}
