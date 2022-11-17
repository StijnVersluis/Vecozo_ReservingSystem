using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Reservation
    {

        public int id { get; set; }
        public int User_id { get; set; }
        public int Workzone_id { get; set; }
        public DateTime DateTime_Arriving { get; set; }
        public DateTime DateTime_Leaving { get; set; }

        public Reservation(int id, int user_id, int workzone_id, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            this.id = id;
            User_id = user_id;
            Workzone_id = workzone_id;
            DateTime_Arriving = dateTime_Arriving;
            DateTime_Leaving = dateTime_Leaving;
        }

        public Reservation(int user_id, int workzone_id, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            User_id = user_id;
            Workzone_id = workzone_id;
            DateTime_Arriving = dateTime_Arriving;
            DateTime_Leaving = dateTime_Leaving;
        }

        public Reservation(ReservationDTO reservationDTO)
        {
            this.id = reservationDTO.Id;
            this.User_id = reservationDTO.User_id;
            this.Workzone_id = reservationDTO.Workzone_id;
            this.DateTime_Arriving = reservationDTO.DateTime_Arriving;
            this.DateTime_Leaving = reservationDTO.DateTime_Leaving;
        }

        public ReservationDTO ToDTO()
        {
            return new ReservationDTO(id, User_id, Workzone_id, DateTime_Arriving, DateTime_Leaving);
        }

        public ReservationDTO ToDTOWithoutId()
        {
            return new ReservationDTO(User_id, Workzone_id, DateTime_Arriving, DateTime_Leaving);
        }
    }
}
