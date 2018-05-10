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
    public class UnitBusiness:IUnitBusiness
    {
        private IUnitRepository _unitRepository;
        public UnitBusiness(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }
        public List<SelectListItem> GetUnitForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Unit> unitList = _unitRepository.GetUnitForSelectList();
            return selectListItem = (from unit in unitList
                                     select new SelectListItem
                                     {
                                         Text = unit.Description,
                                         Value = unit.Code.ToString(),
                                         Selected = false
                                     }).ToList();
        }
    }
}
