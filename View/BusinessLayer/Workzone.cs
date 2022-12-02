using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Workzone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Workspaces { get; set; }
        public int Floor { get; set; }
        public bool TeamOnly { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }

        public Workzone(int id, string name, int workspaces, int floor, bool teamOnly)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
        }
        public Workzone(int id, string name, int workspaces, int floor, bool teamOnly, int xpos, int ypos)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
            Xpos = xpos;
            Ypos = ypos;
        }
        public Workzone(WorkzoneDTO workzone)
        {
            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            Floor = workzone.Floor;
            TeamOnly = workzone.TeamOnly;
            Xpos = workzone.Xpos;
            Ypos = workzone.Ypos;
        }

        public Workzone()
        {
        }
        public WorkzoneDTO toDTO()
        {
            return new WorkzoneDTO(Id, Name, Workspaces, Floor, TeamOnly, Xpos, Ypos);
        }
    }
}
