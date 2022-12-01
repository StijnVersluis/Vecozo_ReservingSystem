using BusinessLayer;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViewLayer.Models
{
    public class WorkzoneViewModel
    {
        public int Id { get; set; }
        [DisplayName("Werkblok")]
        public string Name { get; set; }
        [DisplayName("Aantal werkplekken")]
        public int Workspaces { get; set; }
        [DisplayName("Verdieping")]
        public int Floor { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        [DisplayName("Teamblok")]
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
