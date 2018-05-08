using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IReferencePersonBusiness
    {
        object InsertUpdateReferencePerson(ReferencePerson referencePerson);
        List<ReferencePerson> GetAllReferencePerson(ReferencePersonAdvanceSearch referencePersonAdvanceSearch);
        ReferencePerson GetReferencePerson(int code);
        bool CheckReferencePersonNameExist(ReferencePerson referencePerson);
        object DeleteReferencePerson(int code);
        List<SelectListItem> GetReferencePersonSelectList();
    }
}
