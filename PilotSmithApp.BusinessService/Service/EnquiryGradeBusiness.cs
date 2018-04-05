using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class EnquiryGradeBusiness:IEnquiryGradeBusiness
    {
        IEnquiryGradeRepository _enquiryGradeRepository;
        public EnquiryGradeBusiness(IEnquiryGradeRepository enquiryGradeRepository)
        {
            _enquiryGradeRepository = enquiryGradeRepository;
        }
    }
}
