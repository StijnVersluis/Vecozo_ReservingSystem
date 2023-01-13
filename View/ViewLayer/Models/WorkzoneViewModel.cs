using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViewLayer.Models
{
    public class WorkzoneViewModel
    {
        [Required]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Werkblok")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Aantal werkplekken")]
        [Required]
        public int Workspaces { get; set; }

        [DisplayName("Beschikbare werkplekken")]
        public int AvailableWorkspaces { get; set; }

        [DisplayName("Verdieping")]
        [Required]
        public int Floor { get; set; }

        [DisplayName("Horizontaal")]
        [Required]
        public int Xpos { get; set; }

        [DisplayName("Vertikaal")]
        [Required]
        public int Ypos { get; set; }

        [DisplayName("Teamblok")]
        [Required]
        public bool TeamOnly { get; set; }

        [DisplayName("Verdiepingen")]
        public List<FloorViewModel> Floors { get; set; }

        public bool HasReservations { get; set; } = false;

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

        public WorkzoneViewModel(Workzone workzone, int availableWorkspaces)
        {
            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            AvailableWorkspaces = availableWorkspaces;
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
