using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public  interface IReservationContainer
    {
        bool CreateReservation(ReservationDTO reservationDTO);
        List<ReservationDTO> Getallreservations();
    }
}
