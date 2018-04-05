using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class ReferencePersonBusiness: IReferencePersonBusiness
    {
        IReferencePersonRepository _referencePersonRepository;
        public ReferencePersonBusiness(IReferencePersonRepository referencePersonRepository)
        {
            _referencePersonRepository = referencePersonRepository;
        }
    }
}
