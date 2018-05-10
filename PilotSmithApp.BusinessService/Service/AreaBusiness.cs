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
    public class AreaBusiness:IAreaBusiness
    {
        private IAreaRepository _areaRepository;
        public AreaBusiness(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        public object InsertUpdateArea(Area area)
        {
            return _areaRepository.InsertUpdateArea(area);
        }
        public List<Area> GetAllArea(AreaAdvanceSearch areaAdvanceSearch)
        {
            return _areaRepository.GetAllArea(areaAdvanceSearch);
        }
        public Area GetArea(int code)
        {
            return _areaRepository.GetArea(code);
        }
        public bool CheckAreaNameExist(Area area)
        {
            return _areaRepository.CheckAreaNameExist(area);
        }
        public object DeleteArea(int code)
        {
            return _areaRepository.DeleteArea(code);
        }
        public List<SelectListItem> GetAreaForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Area> areaList = _areaRepository.GetAreaForSelectList();
            return selectListItem = areaList!=null?(from area in areaList
                              select new SelectListItem
                              {
                                  Text = area.Description,
                                  Value = area.Code.ToString(),
                                  Selected = false
                              }).ToList():new List<SelectListItem>();
        }
    }
}
