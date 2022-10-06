using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class TeamDTO
    {
        public string Name { get; set; }
        public TeamDTO(string name)
        {
            Name = name;
        }
    }
}
