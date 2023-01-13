using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }

        public TeamDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public TeamDTO(int id, string name, DateTime? deletedAt)
        {
            Id = id;
            Name = name;
            DeletedAt = deletedAt;
        }
    }
}
