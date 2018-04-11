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
    public class DocumentStatusBusiness:IDocumentStatusBusiness
    {
        IDocumentStatusRepository _documentStatusRepository;
        public DocumentStatusBusiness(IDocumentStatusRepository documentStatusRepository)
        {
            _documentStatusRepository = documentStatusRepository;
        }
        public List<SelectListItem> GetSelectListForDocumentStatus()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<DocumentStatus> documentStatusList = _documentStatusRepository.GetDocumentStatusSelectList();
            if (documentStatusList != null)
                foreach (DocumentStatus documentStatus in documentStatusList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = documentStatus.Description,
                        Value = documentStatus.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
