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
        bool CancelReservation(int id);
        List<ReservationDTO> GetAllReservations();
        public List<ReservationDTO> GetTodayReservationsFromUser(int id);
        public List<ReservationDTO> GetReservationsFromUser(int id);
        public ReservationDTO GetById(int id);
    }
}
