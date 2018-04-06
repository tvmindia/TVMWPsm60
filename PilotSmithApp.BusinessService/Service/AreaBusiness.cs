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
        public bool CheckAreaCodeExist(int code)
        {
            return _areaRepository.CheckAreaCodeExist(code);
        }
        public object DeleteArea(int code)
        {
            return _areaRepository.DeleteArea(code);
        }
        public List<SelectListItem> GetAreaForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Area> areaList = _areaRepository.GetAreaForSelectList();
            if (areaList != null)
                foreach (Area area in areaList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = area.Description,
                        Value = area.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
