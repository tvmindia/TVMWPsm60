using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IEnquiryFollowupBusiness
    {
        object InsertUpdateEnquiryFollowup(EnquiryFollowup enquiryFollowup);
        List<EnquiryFollowup> GetAllEnquiryFollowup(EnquiryFollowup enquiryFollowup);
        EnquiryFollowup GetEnquiryFollowup(Guid id);
        object DeleteEnquiryFollowup(Guid id);
    }
}
