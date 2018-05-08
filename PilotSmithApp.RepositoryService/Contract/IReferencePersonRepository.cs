using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IReferencePersonRepository
    {
        object InsertUpdateReferencePerson(ReferencePerson referencePerson);
        List<ReferencePerson> GetAllReferencePerson(ReferencePersonAdvanceSearch referencePersonAdvanceSearch);
        ReferencePerson GetReferencePerson(int code);
        bool CheckReferencePersonNameExist(ReferencePerson referencePerson);
        object DeleteReferencePerson(int code);
        List<ReferencePerson> GetReferencePersonSelectList();
    }
}
