using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer
{
    public interface IRoleContainer
    {
        public List<RoleDTO> GetRoles();

        public RoleDTO GetRole(int roleId);
    }
}
