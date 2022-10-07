using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            return ireservationContainer.CreateReservation(reservationDTO);
        }

        public List<Reservation> GetAllReservations()
        {
            //List<ReservationDTO> reservationDTO = ireservationContainer.GetAllReservations();
            //List<Reservation> allreservations = new List<Reservation>();
            //foreach (ReservationDTO reservations in reservationDTO)
            //{
            //    allreservations.Add(new Reservation(reservations));
            //}                 
            return ireservationContainer.GetAllReservations().ConvertAll(x => new Reservation(x));
        }
    }
}
