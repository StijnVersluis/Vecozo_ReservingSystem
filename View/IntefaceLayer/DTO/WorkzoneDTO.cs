using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class WorkzoneDTO
    {
        public int Id { get;  set; }
        public string Name { get; set; }
        public int Workspaces { get; set; }
        public int Floor { get; private set; }
        public bool TeamOnly { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }

        public WorkzoneDTO(int id, string name, int workspaces, int floor, bool teamOnly, int xpos, int ypos)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
            Xpos = xpos;
            Ypos = ypos;
        }

        public WorkzoneDTO()
        {
        }
    }
}
