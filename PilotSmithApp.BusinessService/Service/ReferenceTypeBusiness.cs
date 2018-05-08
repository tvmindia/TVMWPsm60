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
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ReferenceType> referencePersonStatusList = _referenceTypeRepository.GetReferenceTypeSelectList();
            if (referencePersonStatusList != null)
                foreach (ReferenceType referencePerson in referencePersonStatusList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = referencePerson.Description,
                        Value = referencePerson.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
