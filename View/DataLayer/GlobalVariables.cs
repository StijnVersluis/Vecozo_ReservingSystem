using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class GlobalVariables
    {
        internal static UserDTO LoggedInUser = new UserDTO(3, "Tim", 1);
    }
}
