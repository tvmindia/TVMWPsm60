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
    public class DocumentTypeBusiness: IDocumentTypeBusiness
    {
        IDocumentTypeRepository _documentTypeRepository;
        public DocumentTypeBusiness(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }
        public List<SelectListItem> GetDocumentTypeSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<DocumentType> documentTypeList = _documentTypeRepository.GetDocumentTypeSelectList();
            if (documentTypeList != null)
                foreach (DocumentType documentType in documentTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = documentType.Description,
                        Value = documentType.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
