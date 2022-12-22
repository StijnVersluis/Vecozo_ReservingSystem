using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ViewLayer.Models
{
    public class WorkzoneReservationViewModel
    {
        private DateTime _dateTimeLeaving = DateTime.Now.ToLocalTime();

        [HiddenInput]
        public string Workzone_Name { get; set; }

        [HiddenInput]
        public int? TeamId { get; set; }

        [Required]
        [HiddenInput]
        public int Workzone_id { get; set; }

        public int Workspaces { get; init; }

        [Required]
        [DisplayName("Van:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTime_Arriving { get; set; } = DateTime.Now;

        [Required]
        [HiddenInput]
        public string RedirectUrl { get; init; } = "/";

        [Required]
        [DisplayName("Tot:")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //DateTime.Now.ToLocalTime();
        public DateTime DateTime_Leaving
        {
            get => _dateTimeLeaving;
            set
            {
                _dateTimeLeaving = new DateTime(
                    DateTime_Arriving.Year, DateTime_Arriving.Month, DateTime_Arriving.Day, 
                    value.Hour, value.Minute, value.Second
                );
            }
        }
    }
}
