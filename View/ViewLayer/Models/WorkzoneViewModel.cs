using BusinessLayer;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViewLayer.Models
{
    public class WorkzoneViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Workspaces { get; set; }
        public int Floor { get; set; }
        public int FloorId { get; set; }    
        public bool TeamOnly { get; set; }
        [DisplayName("Verdiepingen")]
        public List<FloorViewModel> Floors { get; set; }

        public WorkzoneViewModel(Workzone workzone)
        {
            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            Floor = workzone.Floor;
            TeamOnly = workzone.TeamOnly;
            Xpos = workzone.Xpos;
            Ypos = workzone.Ypos;
        }

        public WorkzoneViewModel()
        {
        }
    }
}
