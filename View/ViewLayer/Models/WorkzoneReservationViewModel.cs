using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ViewLayer.Models
{
    public class WorkzoneReservationViewModel
    {
        [HiddenInput]
        public string Workzone_Name { get; set; }

        [Required]
        [HiddenInput]
        public int Workzone_id { get; set; }

        public int Workspaces { get; init; }

        [Required]
        [DisplayName("Aankomsttijd en datum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateTime_Arriving { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Vetrektijd")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime DateTime_Leaving { get; set; } = DateTime.Now.ToLocalTime();
    }
}
