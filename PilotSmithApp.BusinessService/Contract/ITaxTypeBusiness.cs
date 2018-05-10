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
        object InsertUpdateTaxType(TaxType taxType);
        List<TaxType> GetAllTaxType(TaxTypeAdvanceSearch taxTypeAdvanceSearch);
        TaxType GetTaxType(int code);
        bool CheckTaxTypeNameExist(TaxType taxType);
        object DeleteTaxType(int code);
        List<SelectListItem> GetTaxTypeForSelectList();
    }
}
