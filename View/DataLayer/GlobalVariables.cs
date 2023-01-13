using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class GlobalVariables
    {
        public static Dictionary<string, UserDTO> Machines = new();

        internal static UserDTO LoggedInUser() => Machines.ContainsKey(Environment.MachineName) ? Machines[Environment.MachineName] : null;
        //internal static UserDTO LoggedInUser = null;
    }
}
