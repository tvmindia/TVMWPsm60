using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IDistrictRepository
    {
        object InsertUpdateDistrict(District district);
        List<District> GetAllDistrict(DistrictAdvanceSearch districtAdvanceSearch);
        District GetDistrict(int code);
        bool CheckDistrictNameExist(District district);
        object DeleteDistrict(int code);
        List<District> GetDistrictForSelectList(int? stateCode, int? countryCode);
    }
}
