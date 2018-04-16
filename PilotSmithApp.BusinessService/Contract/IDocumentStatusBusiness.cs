using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IDocumentStatusBusiness
    {
        List<SelectListItem> GetSelectListForDocumentStatus(string code);
    }
}
