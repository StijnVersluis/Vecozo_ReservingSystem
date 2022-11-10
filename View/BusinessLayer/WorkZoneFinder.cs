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
        /// <summary>
        /// returns a list of Workzones that are availble between set datetimes and haave enough room for the amount of users
        /// </summary>
        /// <param name="allWorkZones"></param>
        /// <param name="allReservations"></param>
        /// <param name="Users"></param>
        /// <param name="dateTime_Arriving"></param>
        /// <param name="dateTime_Leaving"></param>
        /// <returns>A list of available and free Workzones</returns>
        public List<Workzone> AvailableWorkzones(List<Workzone> allWorkZones, List<Reservation> allReservations, List<User> Users, DateTime dateTime_Arriving, DateTime dateTime_Leaving, bool TeamSelected)
        {
            List<Workzone> freeWorkzone = GetTeamOrIndiviualZones( 
                                                            GetfreeWorkZones(allWorkZones, GetInterferingReservations(allReservations, dateTime_Arriving, dateTime_Leaving), Users)
                                                            , TeamSelected
                                                            );
            return freeWorkzone;
        }
        /// <summary>
        /// Filters the reservations by date to see which are interfering with the given datetimes
        /// </summary>
        /// <param name="allReservations"></param>
        /// <param name="dateTime_Arriving"></param>
        /// <param name="dateTime_Leaving"></param>
        /// <returns>Returns interfering Reservations</returns>
        public List<Reservation> GetInterferingReservations(List<Reservation> allReservations, DateTime dateTime_Arriving, DateTime dateTime_Leaving)
        {
            List<Reservation> filteredReservation;
            filteredReservation = allReservations.Where(reser => dateTime_Arriving <= reser.DateTime_Arriving && reser.DateTime_Arriving <= dateTime_Leaving).ToList(); // checks if any reservation is starting a during the users planned reservation

            filteredReservation.AddRange(allReservations.Where(reser => dateTime_Arriving <= reser.DateTime_Leaving && reser.DateTime_Leaving <= dateTime_Leaving).ToList()); // checks if any reservation is ending a during the users planned reservation

            filteredReservation.AddRange(allReservations.Where(reser => (reser.DateTime_Arriving <= dateTime_Arriving && dateTime_Leaving <= reser.DateTime_Leaving)).ToList()); // checks if planned reservation is happening during a going reservation
            return filteredReservation;
        }
        /// <summary>
        /// Looks for available workzones based on reservations and the amount of users looking for a spot
        /// </summary>
        /// <param name="allWorkZones"></param>
        /// <param name="filteredReservation"></param>
        /// <param name="Users"></param>
        /// <returns>Returns available workzones</returns>
        public List<Workzone> GetfreeWorkZones(List<Workzone> allWorkZones, List<Reservation> filteredReservation, List<User> Users)
        {
            List<Workzone> UnavailableWorkZones = new();
            filteredReservation.ForEach(reser =>
                                                UnavailableWorkZones.AddRange(
                                                            allWorkZones.Where(workZone =>
                                                                workZone.Id == reser.Workzone_id
                                                                ).Where(workZone =>
                                                                        (filteredReservation.Count(reser => reser.Workzone_id == workZone.Id) >= workZone.Workspaces) //Checks if there is any workspace available
                                                                        ||
                                                                        (workZone.Workspaces - filteredReservation.Count(reser => reser.Workzone_id == workZone.Id)) < Users.Count() //Checks if the availabe workspace is enough for the amount of users
                                                                )
                                                            )
                                                ); // makes a list of workzones that are taken or contains to little workspace for the users 
            UnavailableWorkZones.ForEach(workZone => allWorkZones.Remove(workZone)); // removes Unavailable Workzones from list
            return allWorkZones;
        }

        public List<Workzone> GetTeamOrIndiviualZones(List<Workzone> freeWorkzones, bool TeamSelected)
        {
            List<Workzone> returnlist = new();
            if (TeamSelected == true )
            {
                freeWorkzones.RemoveAll(workZone => workZone.Name.StartsWith("ST-WP") ); // ST-WP staat voor de Stilte werkplek als de naam veranderd word voeg dan hier de eerste paar letters van de nieuwe naam in
                returnlist.AddRange(freeWorkzones.Take(3)); //grabs the top 3
            }
            else
            {
                freeWorkzones.RemoveAll(workZone => workZone.TeamOnly == true);
                returnlist.AddRange(freeWorkzones.Where(workZone => workZone.Name.StartsWith("WB")).Take(3));
                returnlist.AddRange(freeWorkzones.Where(workZone => workZone.Name.StartsWith("ST-WP")).Take(2));
            } 
            return returnlist;
        }
    }
}
