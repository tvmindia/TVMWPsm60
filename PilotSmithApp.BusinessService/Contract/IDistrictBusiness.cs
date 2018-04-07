using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IDistrictBusiness
    {
        object InsertUpdateDistrict(District district);
        List<District> GetAllDistrict(DistrictAdvanceSearch districtAdvanceSearch);
        District GetDistrict(int code);
        bool CheckDistrictNameExist(District district);
        object DeleteDistrict(int code);
        List<SelectListItem> GetDistrictForSelectList();
    }
}
