using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
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
        public bool CancelReservation(int id)
        {
            return ireservationContainer.CancelReservation(id);
        }

        public bool CancelReservation(int id)
        {
            return ireservationContainer.CancelReservation(id);
        }

        public List<Reservation> GetAllReservations()
        {             
            return ireservationContainer.GetAllReservations().ConvertAll(x => new Reservation(x));
        }

        public List<Reservation> GetTodayReservationsFromUser(int id)
        {
            return ireservationContainer.GetTodayReservationsFromUser(id).ConvertAll(x => new Reservation(x));
        }

        public List<Reservation> GetReservationsFromUser(int id)
        {
            return ireservationContainer.GetReservationsFromUser(id).ConvertAll(x => new Reservation(x));
        }

        public Reservation GetById(int id)
        {
            Reservation output = null;
            ReservationDTO result = ireservationContainer.GetById(id);

            if (result != null)
            {
                output = new Reservation(result);
            }

            return output;
        }

        public List<string> CheckGeneralRules(Reservation model)
        {
            List<string> messages = new();

            TimeSpan leavingTime = new(model.DateTime_Leaving.Hour, model.DateTime_Leaving.Minute, 0);
            TimeSpan arrivingTime = new(model.DateTime_Arriving.Hour, model.DateTime_Arriving.Minute, 0);
            TimeSpan todayTime = new(DateTime.Now.Hour, DateTime.Now.Minute, 0);

            // Check if the arriving date is greater than or equals todays date.

            // Check if the arriving hours is between 8 am and 17 pm.
            if (!(model.DateTime_Arriving.Hour >= 8 && model.DateTime_Arriving.Hour <= 17))
            {
                messages.Add("De aankomsttijd moet gelijk of later dan 08:00 uur zijn.");
            }

            // Check if the leaving hours is between 9 am and 18 pm.
            if (!(model.DateTime_Leaving.Hour >= 9 && model.DateTime_Leaving.Hour <= 18))
            {
                messages.Add("De vetrektijd moet gelijk of later dan 09:00 uur zijn.");
                messages.Add("De vetrektijd moet gelijk of eerder dan 18:00 uur zijn.");
            }

            // Check if the leaving time is greater than or equals the arrving time.
            if (leavingTime < arrivingTime || leavingTime == arrivingTime)
            {
                messages.Add("Zorg ervoor dat de vetrektijd groter is dan de aankomsttijd!");
            }

            return messages;
        }
    }
}
