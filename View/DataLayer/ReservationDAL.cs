using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
            try
            {
                List<ReservationDTO> reservationlist = new List<ReservationDTO>();

                SqlCommand comand = new SqlCommand("select * from [Book] where Visibility='True'", DBConnection);
                if (comand.Connection.State != ConnectionState.Open)
                {
                    comand.Connection.Open();
                }

                SqlDataReader reader = comand.ExecuteReader();
                while (reader.Read())
                {

                    ReservationDTO reservationDTO = new ReservationDTO();
                    reservationDTO.Id = Convert.ToInt32(reader["ID"]);
                    reservationDTO.User_id = Convert.ToInt32(reader["User_id"]);
                    reservationDTO.Workzone_id = Convert.ToInt32(reader["Workzone_id"]);
                    reservationDTO.DateTime_Arriving = Convert.ToDateTime(reader["DateTime_Arriving"]);
                    reservationDTO.DateTime_Leaving = Convert.ToDateTime(reader["DateTime_Leaving"]);
                    reservationlist.Add(reservationDTO);
                }
                reader.Close();

                return reservationlist;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally

            { DBConnection.Close(); }
        }
    }
}
