using BusinessLayer;
using System;

namespace ViewLayer.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Workzone_id { get; set; }
        public DateTime DateTime_Arriving { get; set; }
        public DateTime DateTime_Leaving { get; set; }
    }
}
