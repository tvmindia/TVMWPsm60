using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class DocumentTypeBusiness: IDocumentTypeBusiness
    {
        IDocumentTypeRepository _documentTypeRepository;
        public DocumentTypeBusiness(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }
    }
}
