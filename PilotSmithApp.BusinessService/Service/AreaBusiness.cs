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
        public List<Area> GetAreaForSelectList()
        {
            return _areaRepository.GetAreaForSelectList();
        }
    }
}
