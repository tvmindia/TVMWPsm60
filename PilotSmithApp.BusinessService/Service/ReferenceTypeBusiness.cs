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
    public class ReferenceTypeBusiness: IReferenceTypeBusiness
    {
        IReferenceTypeRepository _referenceTypeRepository;
        public ReferenceTypeBusiness(IReferenceTypeRepository referenceTypeRepository)
        {
            _referenceTypeRepository = referenceTypeRepository;
        }
        public List<SelectListItem> GetReferenceTypeSelectList()
        {
            List<SelectListItem> selectListItem =null;
            List<ReferenceType> referenceTypeList = _referenceTypeRepository.GetReferenceTypeSelectList();
            return selectListItem = referenceTypeList!=null?(from referenceType in referenceTypeList
                                     select new SelectListItem
                                     {
                                         Text = referenceType.Description,
                                         Value = referenceType.Code.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
    }
}
