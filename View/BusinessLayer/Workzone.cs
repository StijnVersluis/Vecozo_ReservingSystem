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
        public string Name { get;  set; }
        public int Workspaces { get; set; }
        public int Floor { get;  set; }
        public bool TeamOnly { get;  set; }
        public string PositionX { get; set; }
        public string PositionY { get; set; }

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
            PositionX = workzone.PositionX;
            PositionY = workzone.PositionY;
        }
        public Workzone()
        {
        }
    }
}

