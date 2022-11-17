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
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Workspaces { get; private set; }
        public int Floor { get; private set; }
        public bool TeamOnly { get; private set; }

        public Workzone(int id, string name, int workspaces, int floor, bool teamOnly)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
        }
        public Workzone(WorkzoneDTO workzone)
        {
            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            Floor = workzone.Floor;
            TeamOnly = workzone.TeamOnly;
        }
    }
}
