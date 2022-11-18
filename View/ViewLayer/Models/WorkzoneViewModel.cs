using BusinessLayer;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models
{
    public class WorkzoneViewModel
    {
        
        public int Id { get; set; }
        [DisplayName("Naam")]
        public string Name { get; set; }
        [DisplayName("Werkplekken")]
        public int Workspaces { get; set; }
        [DisplayName("Alleen Team")]
        public bool TeamOnly { get; set; }
        [DisplayName("PositieX")]
        public string PositionX { get; set; }
        [DisplayName("PositieY")]
        public string PositionY { get; set; }

        public int Floor { get; set; }

    [DisplayName("Verdiepingen")]
        public List<FloorViewModel> Floors { get; set; }

        public WorkzoneViewModel(Workzone workzone)
        {
            Id= workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            TeamOnly = workzone.TeamOnly;
            PositionX = workzone.PositionX;
            PositionY = workzone.PositionY;
            Floor = workzone.Floor;
        }

        public WorkzoneViewModel()
        {
        }
    }
}
