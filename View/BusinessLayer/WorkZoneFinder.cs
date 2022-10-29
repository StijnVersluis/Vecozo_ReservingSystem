using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class WorkZoneFinder
    {
        public WorkZoneFinder()
        {

        }
        // er gefilterd op datetime op resrvations waar de workzone id word gebruikt om de workzone uit de lijst te halen
        // dan word er gefilter op available places voor aantal users
        
        public List<Workzone> AvailableWorkzones(List<Workzone> allWorkZones, List<Reservation> allReservations, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            return GetfreeWorkZones(allWorkZones, GetInterferingReservations(allReservations, dateTime_Arriving, dateTime_Leaving));
        }

        public List<Reservation> GetInterferingReservations(List<Reservation> allReservations, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            List<Reservation> filteredReservation;
            filteredReservation = allReservations.Where(reser => reser.DateTime_Arriving <= dateTime_Arriving && reser.DateTime_Arriving >= dateTime_Leaving).ToList(); // checks if any reservation is starting a during the users planned reservation
            
            filteredReservation = filteredReservation.Where(reser => reser.DateTime_Leaving >= dateTime_Arriving && reser.DateTime_Leaving <= dateTime_Leaving).ToList(); // checks if any reservation is ending a during the users planned reservation
           
            filteredReservation = filteredReservation.Where(reser => (reser.DateTime_Arriving <= dateTime_Arriving && reser.DateTime_Arriving<= dateTime_Leaving) 
                                                                        &&
                                                                     (reser.DateTime_Leaving >= dateTime_Arriving && reser.DateTime_Leaving >= dateTime_Arriving)).ToList(); // checks if planned reservation is happening during a going reservation
            return filteredReservation;
        }
        public List<Workzone> GetfreeWorkZones(List<Workzone> allWorkZones, List<Reservation> filteredReservation)
        {
            List<Workzone> UnavailableWorkZones = new();
            filteredReservation.ForEach(reser => UnavailableWorkZones.Add(allWorkZones.First(workZone => workZone.Id == reser.Workzone_id))); // makes a list of workzones that are taken
            UnavailableWorkZones.ForEach(workZone => allWorkZones.Remove(workZone)); // removes takenWorkzones from list
            return allWorkZones;
        }
    }
}
