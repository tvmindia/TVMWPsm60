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
        bool CheckAreaCodeExist(int code);
        object DeleteArea(int code);
        List<Area> GetAreaForSelectList();
    }
}
