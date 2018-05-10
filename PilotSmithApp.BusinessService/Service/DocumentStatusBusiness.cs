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
        public List<SelectListItem> GetSelectListForDocumentStatus(string code)
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<DocumentStatus> documentStatusList = _documentStatusRepository.GetDocumentStatusSelectList(code);
            return selectListItem = (from documentStatus in documentStatusList
                                     select new SelectListItem
                                     {
                                         Text = documentStatus.Description,
                                         Value = documentStatus.Code.ToString(),
                                         Selected = false
                                     }).ToList();
        }
    }
}
