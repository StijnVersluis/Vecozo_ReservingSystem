using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class WorkzoneDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Workspaces { get; set; }
        public int Floor { get; private set; }
        public bool TeamOnly { get; private set; }
        public string Xpos { get; private set; }
        public string Ypos { get; private set; }

        public WorkzoneDTO(int id, string name, int workspaces, int floor, bool teamOnly, string xpos, string ypos)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
            Xpos = xpos;
            Ypos = ypos;
        }
    }
}
