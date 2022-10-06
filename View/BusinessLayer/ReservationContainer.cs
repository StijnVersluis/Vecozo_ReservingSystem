using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ReservationContainer
    {
        private IReservationContainer ireservationContainer;
        public ReservationContainer(IReservationContainer ireservationContainer)
        {
            this.ireservationContainer = ireservationContainer;
        }
        public bool CreateReservation(Reservation reservation)
        {
            ReservationDTO reservationDTO = reservation.ToDTO();
            ireservationContainer.CreateReservation(reservationDTO);
            return true;
        }

        public List<Reservation> Getallreservations()
        {
            List<ReservationDTO> reservationDTO = ireservationContainer.Getallreservations();
            List<Reservation> allreservations = new List<Reservation>();
            foreach (ReservationDTO reservations in reservationDTO)
            {
                allreservations.Add(new Reservation(reservations));
            }                 
            return allreservations;
        }
    }
}
