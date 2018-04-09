using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICompanyBusiness
    {
        object InsertUpdateCompany(Company company);
        bool CheckCompanyNameExist(Company company);
        List<Company> GetAllCompany(CompanyAdvanceSearch companyAdvanceSearch);
        Company GetCompany(Guid id);        
        object DeleteCompany(Guid id);
        List<SelectListItem> GetCompanyForSelectList();
    }
}
