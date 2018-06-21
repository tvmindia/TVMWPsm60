using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IAreaBusiness
    {
        object InsertUpdateArea(Area area);
        List<Area> GetAllArea(AreaAdvanceSearch areaAdvanceSearch);
        Area GetArea(int code);
        bool CheckAreaNameExist(Area area);
        object DeleteArea(int code);
        List<SelectListItem> GetAreaForSelectList(int? districtCode=null);
    }
}
