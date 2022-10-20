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

        public Workzone(int id, string name, int workspaces, int floor)
        {
            Id = id;
            Name = name;
            Workspaces = workspaces;
            Floor = floor;
        }
        public Workzone(WorkzoneDTO workzone)
        {
            if (workzone == null) return;

            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            Floor = workzone.Floor;
        }
    }
}
