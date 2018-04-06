﻿using PilotSmithApp.BusinessService.Contract;
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
    public class CompanyBusiness:ICompanyBusiness
    {
        private ICompanyRepository _companyRepository;
        public CompanyBusiness(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public object InsertUpdateCompany(Company company)
        {
            return _companyRepository.InsertUpdateCompany(company);
        }
        public List<Company> GetAllCompany(CompanyAdvanceSearch companyAdvanceSearch)
        {
            return _companyRepository.GetAllCompany(companyAdvanceSearch);
        }
        public Company GetCompany(Guid id)
        {
            return _companyRepository.GetCompany(id);
        }
        public object DeleteCompany(Guid id)
        {
            return _companyRepository.DeleteCompany(id);
        }
        public List<SelectListItem> GetCompanyForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Company> companyList = _companyRepository.GetCompanyForSelectList();
            if (companyList != null)
                foreach (Company company in companyList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = company.Name,
                        Value = company.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

