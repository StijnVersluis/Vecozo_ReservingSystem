using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class ReservationDTO
    {
        public int Id;
        public int User_id;
        public int Workzone_id;
        public DateTime DateTime_Arriving;
        public DateTime DateTime_Leaving;

        public ReservationDTO()
        {
        }

        public ReservationDTO(int id, int user_id, int workzone_id, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            this.Id = id;
            this.User_id = user_id;
            this.Workzone_id = workzone_id;
            this.DateTime_Arriving = dateTime_Arriving;
            this.DateTime_Leaving = dateTime_Leaving;
        }
    }

}
