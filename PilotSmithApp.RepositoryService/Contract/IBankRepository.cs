using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IBankRepository
    {
        List<Bank> GetBankForSelectList();
        List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch);
        Bank GetBank(int code);
        bool CheckBankExist(Bank bank);
        object InsertUpdateBank(Bank bank);
        object DeleteBank(int code);
    }
}
