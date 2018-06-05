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
    public class BankBusiness:IBankBusiness
    {
        private IBankRepository _bankRepository;
        public BankBusiness(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }
        public List<SelectListItem> GetBankForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Bank> bankList= _bankRepository.GetBankForSelectList();
            return selectListItem = bankList != null ? (from bank in bankList
                                                        select new SelectListItem
                                                        {
                                                            Text = bank.Name,
                                                            Value = bank.Code,
                                                            Selected = false,
                                                        }).ToList() : new List<SelectListItem>();
        }
        public List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch)
        {
            return _bankRepository.GetAllBank(bankAdvanceSearch);
        }
        public object InsertUpdateBank(Bank bank)
        {
            return _bankRepository.InsertUpdateBank(bank);
        }
        public Bank GetBank(string code)
        {
            return _bankRepository.GetBank(code);
        }
        public bool CheckCodeExist(string code)
        {
            return _bankRepository.CheckCodeExist(code);
        }
        public object DeleteBank(string code)
        {
            return _bankRepository.DeleteBank(code);
        }
    }
}
