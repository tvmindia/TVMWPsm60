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
    public class TaxTypeBusiness:ITaxTypeBusiness
    {
        private ITaxTypeRepository _taxTypeRepository;
        public TaxTypeBusiness(ITaxTypeRepository taxTypeRepository)
        {
            _taxTypeRepository = taxTypeRepository;
        }
        public object InsertUpdateTaxType(TaxType taxType)
        {
            return _taxTypeRepository.InsertUpdateTaxType(taxType);
        }
        public List<TaxType> GetAllTaxType(TaxTypeAdvanceSearch taxTypeAdvanceSearch)
        {
            return _taxTypeRepository.GetAllTaxType(taxTypeAdvanceSearch);
        }
        public TaxType GetTaxType(int code)
        {
            return _taxTypeRepository.GetTaxType(code);
        }
        public bool CheckTaxTypeNameExist(TaxType taxType)
        {
            return _taxTypeRepository.CheckTaxTypeNameExist(taxType);
        }
        public object DeleteTaxType(int code)
        {
            return _taxTypeRepository.DeleteTaxType(code);
        }
        public List<SelectListItem> GetTaxTypeForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<TaxType> taxTypeList = _taxTypeRepository.GetTaxTypeForSelectList();
            return selectListItem = taxTypeList!=null?(from taxType in taxTypeList
                                     select new SelectListItem
                                     {
                                         Text = taxType.Description,
                                         Value = taxType.ValueText.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();


        }
    }
}
