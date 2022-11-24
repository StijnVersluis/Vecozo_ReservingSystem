using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class RoleContainer
    {
        private IRoleContainer iRoleContainer;
        public RoleContainer(IRoleContainer iroleContainer)
        {
            this.iRoleContainer = iroleContainer;
        }

        public List<Role> GetRoles()
        {
            return iRoleContainer.GetRoles().ConvertAll(roleDTO => new Role(roleDTO));
        }

        public Role GetRole(int roleId)
        {
            return new Role(iRoleContainer.GetRole(roleId));
        }
    }
}
