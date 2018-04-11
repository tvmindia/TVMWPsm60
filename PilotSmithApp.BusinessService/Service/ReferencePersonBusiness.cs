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
    public class ReferencePersonBusiness: IReferencePersonBusiness
    {
        IReferencePersonRepository _referencePersonRepository;
        public ReferencePersonBusiness(IReferencePersonRepository referencePersonRepository)
        {
            _referencePersonRepository = referencePersonRepository;
        }
        public List<SelectListItem> GetReferencePersonSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ReferencePerson> referencePersonStatusList = _referencePersonRepository.GetReferencePersonSelectList();
            if (referencePersonStatusList != null)
                foreach (ReferencePerson referencePerson in referencePersonStatusList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = referencePerson.Name,
                        Value = referencePerson.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
