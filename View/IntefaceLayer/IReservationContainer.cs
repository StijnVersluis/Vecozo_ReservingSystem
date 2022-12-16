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
        bool CreateTeamReservation(TeamReservationDTO reservationDTO);
        bool CancelReservation(int id);
        List<ReservationDTO> GetAllReservations();
        public List<ReservationDTO> GetReservationsFromUser(int id);
        public List<ReservationDTO> GetReservationsFromWorkzone(int id);
        public List<ReservationDTO> GetReservationsWithinTimeFrame(DateTime timeFrameStart, DateTime timeFrameEnd);
        public List<TeamReservationDTO> GetTeamReservationsWithinTimeFrame(DateTime timeFrameStart, DateTime timeFrameEnd);
        public ReservationDTO GetById(int id);
    }
}
