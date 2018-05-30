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
    public class PositionBusiness : IPositionBusiness
    {
        IPositionRepository _positionRepository;
        public PositionBusiness(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public List<SelectListItem> GetPositionSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Position> positionList = _positionRepository.GetPositionSelectList();
            return selectListItem = positionList != null ? (from position in positionList
                                                                    select new SelectListItem
                                                                    {
                                                                        Text = position.Description,
                                                                        Value = position.Code.ToString(),
                                                                        Selected = false
                                                                    }).ToList() : new List<SelectListItem>();
        }
    }
}
