using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class UserDTO
    {
        public int Id { private set; get; }
        public string Name { private set; get; }
        public int Role { private set; get; }

        public UserDTO(int id, string name, int role)
        {
            Id = id;
            Name = name;
            Role = role;
        }
    }
}
