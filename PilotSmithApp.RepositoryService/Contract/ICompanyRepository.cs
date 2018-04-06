using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ICompanyRepository
    {
        object InsertUpdateCompany(Company company);
        List<Company> GetAllCompany(CompanyAdvanceSearch companyAdvanceSearch);
        Company GetCompany(Guid id);
        object DeleteCompany(Guid id);
        List<Company> GetCompanyForSelectList();
    }
}
