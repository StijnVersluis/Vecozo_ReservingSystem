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
        private SqlDataReader reader;
        public ReservationDAL()
        {
            InitializeDB();
        }

        public List<ReservationDTO> GetReservationsFromWorkzone(int id)
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Reservations WHERE Workzone_Id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                var WorkzoneReservationReader = DbCom.ExecuteReader();
                while (WorkzoneReservationReader.Read())
                {
                    reservations.Add(new ReservationDTO((int)WorkzoneReservationReader["Id"],
                        (int)WorkzoneReservationReader["User_Id"],
                        (int)WorkzoneReservationReader["Workzone_Id"],
                        (DateTime)WorkzoneReservationReader["DateTime_Arriving"],
                        (DateTime)WorkzoneReservationReader["DateTime_Leaving"]));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally { CloseCon(); }
            return reservations;
        }
        public List<ReservationDTO> GetReservationsWithinTimeFrame(DateTime timeFrameStart, DateTime timeFrameEnd)
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Reservations WHERE (DateTime_Arriving BETWEEN @startday and @endday)" +
                    " OR (DateTime_Leaving BETWEEN @startday and @endday)";
                DbCom.Parameters.AddWithValue("startday", timeFrameStart);
                DbCom.Parameters.AddWithValue("endday", timeFrameEnd);

                var WorkzoneReservationReader = DbCom.ExecuteReader();
                while (WorkzoneReservationReader.Read())
                {
                    reservations.Add(new ReservationDTO((int)WorkzoneReservationReader["Id"],
                        (int)WorkzoneReservationReader["User_Id"],
                        (int)WorkzoneReservationReader["Workzone_Id"],
                        (DateTime)WorkzoneReservationReader["DateTime_Arriving"],
                        (DateTime)WorkzoneReservationReader["DateTime_Leaving"]));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally { CloseCon(); }
            return reservations;
        }

        // Fix this not right
        public bool CreateReservation(ReservationDTO reservationDTO)
        {
            try
            {
                OpenCon();
                DbCom.CommandText = "INSERT INTO Reservations (User_Id, Workzone_Id, DateTime_Arriving, DateTime_Leaving) VALUES (@User_id, @Workzone_id, @DateTime_Arriving, @DateTime_Leaving)";

                DbCom.Parameters.AddWithValue("@User_id", reservationDTO.User_id);
                DbCom.Parameters.AddWithValue("@Workzone_id", reservationDTO.Workzone_id);
                DbCom.Parameters.AddWithValue("@DateTime_Arriving", reservationDTO.DateTime_Arriving);
                DbCom.Parameters.AddWithValue("@DateTime_Leaving ", reservationDTO.DateTime_Leaving);

                DbCom.ExecuteNonQuery();
                CloseCon();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
            finally
            {
                CloseCon();
            }

        }

        public bool CancelReservation(int id)
        {
            OpenCon();

            DbCom.CommandText = "DELETE FROM Reservations WHERE id = @id";
            DbCom.Parameters.AddWithValue("id", id);

            if (DbCom.ExecuteNonQuery() > 0) return true; else return false;
        }

        public List<ReservationDTO> GetAllReservations()
        {
            try
            {
                OpenCon();
                List<ReservationDTO> reservationlist = new List<ReservationDTO>();

                DbCom.CommandText = "SELECT * FROM Reservations";

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    reservationlist.Add(new((int)reader["Id"], (int)reader["User_Id"], (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }
                reader.Close();

                return reservationlist;
            }
            finally
            {
                CloseCon();
            }
        }

        public List<ReservationDTO> GetReservationsFromUser(int id)
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Reservations WHERE User_Id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(new ReservationDTO((int)reader["Id"], id, (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }

                CloseCon();
            }
            finally
            {
                CloseCon();
            }

            return reservations;
        }

        public ReservationDTO GetById(int id)
        {
            ReservationDTO reservation = null;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Reservations WHERE Id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    reservation = new ReservationDTO((int)reader["Id"], id, (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]);
                }

                CloseCon();
            }
            finally
            {
                CloseCon();
            }

            return reservation;
        }
    }
}
