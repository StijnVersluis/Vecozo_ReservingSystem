﻿using IntefaceLayer;
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

        public bool CreateReservation(ReservationDTO reservationDTO)
        {
            try
            {
                OpenCon();

                DbCom.CommandText = "SELECT Workspaces FROM Workzones WHERE Id = @id";
                DbCom.Parameters.AddWithValue("@id", reservationDTO.Workzone_id);

                reader = DbCom.ExecuteReader();
                int workzonePlaces = 0;
                while (reader.Read())
                {
                    workzonePlaces = (int)reader["Workspaces"];
                }

                CloseCon();
                OpenCon();

                DbCom.CommandText = "SELECT * FROM Reservations WHERE Workzone_Id = @workzone_id and @datetime BETWEEN DateTime_Arriving and DateTime_Leaving";
                DbCom.Parameters.AddWithValue("@workzone_id", reservationDTO.Workzone_id);
                DbCom.Parameters.AddWithValue("@datetime", reservationDTO.DateTime_Arriving);

                List<ReservationDTO> ExistingReservations = new List<ReservationDTO>();

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    ExistingReservations.Add(new((int)reader["User_Id"], (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }

                CloseCon();

                bool IsReserved = false;
                foreach (ReservationDTO reservation in ExistingReservations)
                {
                    if (reservation.User_id == reservationDTO.User_id)
                    {
                        IsReserved = true;
                        break;
                    }
                }
                if (!IsReserved && ExistingReservations.Count < workzonePlaces)
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
                else return false;
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

                DbCom.CommandText = "select * from Reservations";

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    reservationlist.Add(new ReservationDTO((int)reader["Id"], (int)reader["User_Id"], (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }
                reader.Close();

                return reservationlist;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { CloseCon(); }
        }

        public List<ReservationDTO> GetDateReservationsFromUser(int id, DateTime dateTime)
        {
            OpenCon();
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            DbCom.CommandText = "SELECT * FROM Reservations WHERE User_Id = @id";

            DbCom.Parameters.AddWithValue("id", id);

            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                DateTime reservationDate = (DateTime)reader["DateTime_Arriving"];

                if (DateTime.Compare(reservationDate.Date, dateTime.Date) == 0)
                {
                    reservations.Add(new ReservationDTO((int)reader["Id"], id, (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }
            }

            CloseCon();

            return reservations;
        }

        public List<ReservationDTO> GetReservationsFromUser(int id)
        {
            OpenCon();
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            DbCom.CommandText = "SELECT * FROM Reservations WHERE User_Id = @id";

            DbCom.Parameters.AddWithValue("id", id);

            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(new ReservationDTO((int)reader["Id"], id, (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
            }

            CloseCon();

            return reservations;
        }
    }
}
