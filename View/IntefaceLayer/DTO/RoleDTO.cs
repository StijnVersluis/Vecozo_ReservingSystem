using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer
{
    public class RoleDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public RoleDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
