using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Floor
    {
        public int Id;
        public string Name { get; set; }

        public Floor(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Floor(FloorDTO floorDTO)
        {
            Id = floorDTO.Id;
            Name = floorDTO.Name;
        }
    }
}
