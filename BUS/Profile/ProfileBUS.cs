using DAO.ProfileDAO;
using DTO.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Profile
{
    public class ProfileBUS
    {

        public ProfileDTO? GetProfileByAccountId(int accountId)
        {
            ProfileDAO _dao = new ProfileDAO();
            
            return _dao.GetProfileByAccountId(accountId);
        }
    }

}
