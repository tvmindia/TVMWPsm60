using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ITaxTypeBusiness
    {
        TaxType GetTaxType(int code);
        List<SelectListItem> GetTaxTypeForSelectList();
    }
}
