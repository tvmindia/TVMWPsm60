using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IAreaRepository
    {
        object InsertUpdateArea(Area area);
        List<Area> GetAllArea(AreaAdvanceSearch areaAdvanceSearch);
        Area GetArea(int code);
        bool CheckAreaNameExist(Area area);
        object DeleteArea(int code);
        List<Area> GetAreaForSelectList(int? districtCode, int? stateCode, int? countryCode);
    }
}
