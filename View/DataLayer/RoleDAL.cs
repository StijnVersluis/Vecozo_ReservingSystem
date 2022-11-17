using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RoleDAL : SqlConnect, IRoleContainer
    {
        public RoleDAL() { InitializeDB(); }
        public RoleDTO GetRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public List<RoleDTO> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
