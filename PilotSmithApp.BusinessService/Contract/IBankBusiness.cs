using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IBankBusiness
    {
        List<SelectListItem> GetBankForSelectList();
        List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch);
        Bank GetBank(int code);
        bool CheckBankExist(Bank bank);
        object InsertUpdateBank(Bank bank);
        object DeleteBank(int code);
    }
}
