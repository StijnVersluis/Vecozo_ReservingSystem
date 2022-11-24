using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class FloorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FloorDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
