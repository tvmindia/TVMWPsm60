using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IEnquiryFollowupRepository
    {
        object InsertUpdateEnquiryFollowup(EnquiryFollowup enquiryFollowup);
        List<EnquiryFollowup> GetAllEnquiryFollowup(EnquiryFollowup enquiryFollowup);
        EnquiryFollowup GetEnquiryFollowup(Guid id);
        object DeleteEnquiryFollowup(Guid id);
    }
}
