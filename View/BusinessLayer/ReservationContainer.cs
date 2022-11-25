using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections;
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

        public List<Reservation> GetAllReservations()
        {
            return ireservationContainer.GetAllReservations().ConvertAll(x => new Reservation(x));
        }

        public List<Reservation> GetReservationsFromUser(int id)
        {
            return ireservationContainer.GetReservationsFromUser(id).ConvertAll(x => new Reservation(x));
        }
        /// <summary>
        /// Get all reservations from a specific workzone.
        /// </summary>
        /// <param name="id">The id of the workzone.</param>
        /// <returns>A List of reservations from the workzone.</returns>
        public List<Reservation> GetReservationsFromWorkzone(int id)
        {
            return ireservationContainer.GetReservationsFromWorkzone(id).ConvertAll(reservationDTO => new Reservation(reservationDTO));
        }

        /// <summary>
        /// Get the reservations within the given timeframe.
        /// </summary>
        /// <param name="timeFrameStart">The starting date and time.</param>
        /// <param name="timeFrameEnd">The ending date and time.</param>
        /// <returns>A list of reservations within the given timeframe.</returns>
        public List<Reservation> GetReservationsWithinTimeFrame(DateTime timeFrameStart, DateTime timeFrameEnd)
        {
            return ireservationContainer.GetReservationsWithinTimeFrame(timeFrameStart, timeFrameEnd).ConvertAll(reservationDTO => new Reservation(reservationDTO));
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

        /// <summary>
        /// Check the "new" reservation for errors and if it is possible on the corresponding workzone.
        /// </summary>
        /// <param name="newReservation">The "new" reservation.</param>
        /// <param name="workzone">The corresponding workzone</param>
        /// <returns>An empty list is succesfull or a list of error messages.</returns>
        public List<string> CheckReservationRules(Reservation newReservation, Workzone workzone)
        {
            List<string> messages = new();//Define the result of the function

            //If workzone doesn't exist
            if (workzone == null) { messages.Add($"{workzone.Name} kan niet gereserveerd worden, probeer het later nog eens."); return messages; }
            //If workzone is TeamOnly
            if (workzone.TeamOnly) { messages.Add($"{workzone.Name} kan niet gereserveerd worden, het is alleen voor een team bedoeld."); return messages; }


            //if (GetReservationsFromUser(newReservation.User_id).Any(reservation => DateTimeBetweenTimeFrame(reservation, ))) ;

            List<Reservation> existingReservationsInTimeFrame = GetReservationsWithinTimeFrame(newReservation.DateTime_Arriving, newReservation.DateTime_Leaving);

            if(existingReservationsInTimeFrame.Any(reservation => reservation.User_id == newReservation.User_id))
            { messages.Add($"U heeft al een reservering op {workzone.Name} binnen dit timeframe."); return messages; }
            if (workzone.Workspaces <= existingReservationsInTimeFrame.Where(reservation => reservation.Workzone_id == workzone.Id).Count())
            { messages.Add($"Er zijn geen werkplekken meer beschikbaar op {workzone.Name}."); return messages; }

            // Check if the arriving hours is between 8 am and 17 pm.
            if (newReservation.DateTime_Arriving.Hour < 8) messages.Add("De aankomsttijd moet later dan 08:00 zijn.");
            if (newReservation.DateTime_Arriving.Hour > 17) messages.Add("De aankomsttijd moet voor 17:00 zijn.");

            // Check if the leaving hours is between 9 am and 18 pm.
            if (newReservation.DateTime_Leaving.Hour < 9) messages.Add("De vetrektijd moet gelijk of later dan 09:00 uur zijn.");
            if (newReservation.DateTime_Leaving.Hour > 18) messages.Add("De vetrektijd moet gelijk of eerder dan 18:00 uur zijn.");

            // Check if the leaving time is greater than or equals the arrving time.
            if (newReservation.DateTime_Leaving <= newReservation.DateTime_Arriving) messages.Add("Zorg ervoor dat de vetrektijd groter is dan de aankomsttijd!");

            return messages;
        }
    }
}
