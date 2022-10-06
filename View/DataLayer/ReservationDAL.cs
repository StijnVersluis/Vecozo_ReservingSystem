using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationDAL : SqlConnect, IReservationContainer
    {
        public bool CreateReservation(ReservationDTO reservationDTO)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO Reservations(User_id, Workzone_id, DateTime_Arriving, DateTime_Leaving) VALUES(Reservations(@User_id, @Workzone_id, @DateTime_Arriving, @DateTime_Leaving)", DBConnection);
                DBConnection.Open();
                sqlCommand.Parameters.AddWithValue("@User_id", reservationDTO.User_id);
                sqlCommand.Parameters.AddWithValue("@Workzone_id", reservationDTO.User_id);
                sqlCommand.Parameters.AddWithValue("@DateTime_Arriving", reservationDTO.User_id);
                sqlCommand.Parameters.AddWithValue("@DateTime_Leaving ", reservationDTO.User_id);


                sqlCommand.ExecuteNonQuery();
                return true;
            }

            catch (Exception exception)
            {
                throw exception;
            } 
            finally
            {
                DBConnection.Close();
            }
            
        }

        public List<ReservationDTO> Getallreservations()
        {
            throw new NotImplementedException();
        }
    }
}
