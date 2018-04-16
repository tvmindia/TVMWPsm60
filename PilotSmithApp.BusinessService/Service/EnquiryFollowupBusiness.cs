using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class EnquiryFollowupBusiness: IEnquiryFollowupBusiness
    {
        IEnquiryFollowupRepository _enquiryFollowupRepository;
        public EnquiryFollowupBusiness(IEnquiryFollowupRepository enquiryFollowupRepository)
        {
            _enquiryFollowupRepository = enquiryFollowupRepository;
        }
        public object InsertUpdateEnquiryFollowup(EnquiryFollowup enquiryFollowup)
        {
            return _enquiryFollowupRepository.InsertUpdateEnquiryFollowup(enquiryFollowup);
        }
        public List<EnquiryFollowup> GetAllEnquiryFollowup(EnquiryFollowup enquiryFollowup)
        {
            return _enquiryFollowupRepository.GetAllEnquiryFollowup(enquiryFollowup);
        }
        public EnquiryFollowup GetEnquiryFollowup(Guid id)
        {
            return _enquiryFollowupRepository.GetEnquiryFollowup(id);
        }
    }
}
