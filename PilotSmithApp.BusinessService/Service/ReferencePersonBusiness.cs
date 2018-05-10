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
        public object InsertUpdateReferencePerson(ReferencePerson referencePerson)
        {
            return _referencePersonRepository.InsertUpdateReferencePerson(referencePerson);
        }
        public List<ReferencePerson> GetAllReferencePerson(ReferencePersonAdvanceSearch referencePersonAdvanceSearch)
        {
            return _referencePersonRepository.GetAllReferencePerson(referencePersonAdvanceSearch);
        }
        public ReferencePerson GetReferencePerson(int code)
        {
            return _referencePersonRepository.GetReferencePerson(code);
        }
        public bool CheckReferencePersonNameExist(ReferencePerson referencePerson)
        {
            return _referencePersonRepository.CheckReferencePersonNameExist(referencePerson);
        }
        public object DeleteReferencePerson(int code)
        {
            return _referencePersonRepository.DeleteReferencePerson(code);
        }
        public List<SelectListItem> GetReferencePersonSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ReferencePerson> referencePersonStatusList = _referencePersonRepository.GetReferencePersonSelectList();
            return selectListItem = (from referencePerson in referencePersonStatusList
                              select new SelectListItem
                              {
                                  Text = referencePerson.Name,
                                  Value = referencePerson.Code.ToString(),
                                  Selected = false
                              }).ToList();
        }
    }
}
