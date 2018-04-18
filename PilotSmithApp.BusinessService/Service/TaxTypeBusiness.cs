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
        public TaxType GetTaxType(int code)
        {
            return _taxTypeRepository.GetTaxType(code);
        }
        public List<SelectListItem> GetTaxTypeForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<TaxType> taxTypeList = _taxTypeRepository.GetTaxTypeForSelectList();
            if (taxTypeList != null)
                foreach (TaxType taxType in taxTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = taxType.Description,
                        Value = taxType.Text.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;

        }
    }
}
