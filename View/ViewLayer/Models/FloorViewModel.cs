using BusinessLayer;
using System;

namespace ViewLayer.Models
{
    public class FloorViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public FloorViewModel(Floor floor)
        {
            this.Id = floor.Id;
            this.Name = floor.Name;
        }
    }
}
