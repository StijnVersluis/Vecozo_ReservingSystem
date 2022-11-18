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
        public string Name { get;  set; }
        public int Workspaces { get; set; }
        public int Floor { get;  set; }
        public bool TeamOnly { get;  set; }
        public string PositionX { get; set; }
        public string PositionY { get; set; }

        public WorkzoneDTO(int id, string name, int workspaces, int floor, bool teamOnly )
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
          
        }

        public WorkzoneDTO(int id, string name, int workspaces, int floor, bool teamOnly, string positionX, string positionY)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
            TeamOnly = teamOnly;
            PositionX = positionX;
            PositionY = positionY;
        }

        public WorkzoneDTO()
        {
        }
    }
}
