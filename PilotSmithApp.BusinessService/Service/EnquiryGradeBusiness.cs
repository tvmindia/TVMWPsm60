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
    public class EnquiryGradeBusiness:IEnquiryGradeBusiness
    {
        IEnquiryGradeRepository _enquiryGradeRepository;
        public EnquiryGradeBusiness(IEnquiryGradeRepository enquiryGradeRepository)
        {
            _enquiryGradeRepository = enquiryGradeRepository;
        }
        public List<SelectListItem> GetEnquiryGradeSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<EnquiryGrade> enquiryGradeList = _enquiryGradeRepository.GetEnquiryGradeSelectList();            
            return selectListItem = enquiryGradeList != null?(from enquiryGrade in enquiryGradeList
                                     select new SelectListItem
                                     {
                                         Text = enquiryGrade.Description,
                                         Value = enquiryGrade.Code.ToString(),
                                         Selected = false
                                     }).ToList(): new List<SelectListItem>();
        }
    }
}
