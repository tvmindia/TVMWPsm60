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
    public class UserInBranchBusiness : IUserInBranchBusiness
    {
        private IUserInBranchRepository _userInBranchRepository;

        public UserInBranchBusiness(IUserInBranchRepository userInBranchRepository)
        {
            _userInBranchRepository = userInBranchRepository;
        }
        public List<UserInBranch> GetAllUserInBranchByUserId(Guid userId)
        {
            return _userInBranchRepository.GetAllUserInBranchByUserId(userId);
        }
        public object InsertUpdateUserInBranch(UserInBranch userInBranch)
        {
            return _userInBranchRepository.InsertUpdateUserInBranch(userInBranch);
        }
    }
}
