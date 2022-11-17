using IntefaceLayer.DTO;
using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Role
    {
        private int Id;
        public string Name { get; set; }

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Role(RoleDTO roleDTO)
        {
            Id = roleDTO.Id;
            Name = roleDTO.Name;
        }
    }
}
