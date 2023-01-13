using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
                Debug.WriteLine(e.Message);
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
                Debug.WriteLine(e.Message);
            }
            finally { CloseCon(); }
            return reservations;
        }

        public List<TeamReservationDTO> GetTeamReservationsWithinTimeFrame(DateTime timeFrameStart, DateTime timeFrameEnd)
        {
            List<TeamReservationDTO> result = new();
            try
            {
                if (DbCom.Connection.State == ConnectionState.Closed) OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText = "SELECT Id, Team_Id, DateTime_Arriving, DateTime_Leaving FROM Teamreservations " +
                    " WHERE (DateTime_Arriving BETWEEN @startday and @endday)" +
                    " OR (DateTime_Leaving BETWEEN @startday and @endday)";
                DbCom.Parameters.AddWithValue("startday", timeFrameStart);
                DbCom.Parameters.AddWithValue("endday", timeFrameEnd);

                var reservationReader = DbCom.ExecuteReader();

                while (reservationReader.Read())
                {
                    var teamDTO = new TeamReservationDTO();
                    teamDTO.Id = (int)reservationReader["Id"];
                    teamDTO.TeamId = (int)reservationReader["Team_Id"];
                    teamDTO.TimeArriving = DateTime.Parse(reservationReader["DateTime_Arriving"].ToString());
                    teamDTO.TimeLeaving = DateTime.Parse(reservationReader["DateTime_Leaving"].ToString());
                    result.Add(teamDTO);
                }
                result.ForEach(teamDTO =>
                {
                    //DbCom = new SqlCommand();
                    DbCom.Connection = DBConnection;
                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "SELECT DateTime_Arriving, DateTime_Leaving, Teamreservation_Id, Workzone_Id FROM Teamreservations" +
                    " INNER JOIN TeamreservationsWorkzones ON Teamreservations.Id = TeamreservationsWorkzones.Teamreservation_Id" +
                    " WHERE Teamreservation_Id = @tId AND" +
                    " ((DateTime_Arriving BETWEEN @startday and @endday)" +
                    " OR (DateTime_Leaving BETWEEN @startday and @endday))";
                    DbCom.Parameters.AddWithValue("tId", teamDTO.Id);
                    DbCom.Parameters.AddWithValue("startday", timeFrameStart);
                    DbCom.Parameters.AddWithValue("endday", timeFrameEnd);
                    var teamWorkzoneReader = DbCom.ExecuteReader();
                    while (teamWorkzoneReader.Read())
                    {
                        teamDTO.WorkzoneIds.Add((int)teamWorkzoneReader["Workzone_Id"]);
                    }
                    //DbCom = new SqlCommand();
                    DbCom.Connection = DBConnection;
                    DbCom.CommandText = "SELECT DateTime_Arriving, DateTime_Leaving, Teamreservation_Id, User_Id FROM Teamreservations " +
                    " INNER JOIN TeamreservationUsers ON Teamreservations.Id = TeamreservationUsers.Teamreservation_Id" +
                    " WHERE Teamreservation_Id = @tId AND" +
                    " ((DateTime_Arriving BETWEEN @startday and @endday)" +
                    " OR (DateTime_Leaving BETWEEN @startday and @endday))";
                    DbCom.Parameters.AddWithValue("tId", teamDTO.Id);
                    DbCom.Parameters.AddWithValue("startday", timeFrameStart);
                    DbCom.Parameters.AddWithValue("endday", timeFrameEnd);
                    var teamUserReader = DbCom.ExecuteReader();
                    while (teamUserReader.Read())
                    {
                        teamDTO.UserIds.Add((int)teamUserReader["User_Id"]);
                    }
                });
            }
            catch (Exception) { return new(); }
            finally { CloseCon(); }
            return result;
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
            bool result = false;

            try
            {
                OpenCon();

                DbCom.CommandText = "DELETE FROM Reservations WHERE id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                if (DbCom.ExecuteNonQuery() > 0) return result = true; else result = false;
            }
            catch { }

            return result;
        }

        public bool CancelTeamReservation(int id)
        {
            bool result = false;

            try
            {
                OpenCon();

                DbCom.CommandText = "DELETE FROM Teamreservations WHERE id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                if (DbCom.ExecuteNonQuery() > 0) return result = true; else result = false;
            }
            catch { }

            return result;
        }

        public List<ReservationDTO> GetAllReservations()
        {
            List<ReservationDTO> reservationlist = new List<ReservationDTO>();

            try
            {
                OpenCon();

                DbCom.CommandText = "SELECT * FROM Reservations";

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    reservationlist.Add(new((int)reader["Id"], (int)reader["User_Id"], (int)reader["Workzone_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"]));
                }
                reader.Close();

                return reservationlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                CloseCon();
            }

            return reservationlist;
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                CloseCon();
            }

            return reservations;
        }

        public List<TeamReservationDTO> GetReservationsFromTeam(int userId)
        {
            List<TeamReservationDTO> reservations = new List<TeamReservationDTO>();
            List<int> teamIds = new List<int>();

            try
            {
                OpenCon();

                // Insert team ids of user.
                DbCom.CommandText = "SELECT Team_Id FROM TeamMembers WHERE User_Id = @user";
                DbCom.Parameters.AddWithValue("user", userId);
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    teamIds.Add((int)reader["Team_Id"]);
                }

                CloseCon();

                // Get team reservations.
                teamIds.ForEach(x =>
                {
                    OpenCon();
                    DbCom.Parameters.Clear();
                    DbCom.CommandText =
                        "SELECT a.Id, a.Team_Id, a.DateTime_Arriving, a.DateTime_Leaving, b.Workzone_Id " +
                        "FROM Teamreservations a " +
                        "INNER JOIN TeamreservationsWorkzones b " +
                        "ON b.Teamreservation_Id = a.Id " +
                        "WHERE a.Team_Id = @team";

                    DbCom.Parameters.AddWithValue("team", x);
                    reader = DbCom.ExecuteReader();

                    while (reader.Read())
                    {
                        reservations.Add(new TeamReservationDTO((int)reader["Id"], (int)reader["Team_Id"], (DateTime)reader["DateTime_Arriving"], (DateTime)reader["DateTime_Leaving"], new List<int> { (int)reader["Workzone_Id"] }));
                    }
                    CloseCon();
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                if (DbCom.Connection.State == ConnectionState.Open)
                {
                    CloseCon();
                }
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                CloseCon();
            }

            return reservation;
        }

        public bool CreateTeamReservation(TeamReservationDTO reservationDTO)
        {
            bool success = false;
            int successFullInserts = 0;
            try
            {


                OpenCon();
                DbCom.CommandText = "INSERT INTO Teamreservations (Team_Id, DateTime_Arriving, DateTime_Leaving) Values (@team, @arrive, @leave) SELECT SCOPE_IDENTITY()";
                DbCom.Parameters.AddWithValue("team", reservationDTO.TeamId);
                DbCom.Parameters.AddWithValue("arrive", reservationDTO.TimeArriving);
                DbCom.Parameters.AddWithValue("leave", reservationDTO.TimeLeaving);
                decimal insertedId = (decimal)DbCom.ExecuteScalar();
                if (insertedId > 0) successFullInserts++;

                reservationDTO.WorkzoneIds.ForEach(workzoneId =>
                {
                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "INSERT INTO TeamreservationsWorkzones (Teamreservation_Id, Workzone_Id) Values (@team, @workzone)";
                    DbCom.Parameters.AddWithValue("team", insertedId);
                    DbCom.Parameters.AddWithValue("workzone", workzoneId);
                    if (DbCom.ExecuteNonQuery() > 0) successFullInserts++;
                });

                reservationDTO.UserIds.ForEach(userId =>
                {
                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "INSERT INTO TeamreservationUsers (Teamreservation_Id, User_Id) Values (@team, @user)";
                    DbCom.Parameters.AddWithValue("team", insertedId);
                    DbCom.Parameters.AddWithValue("user", userId);
                    if (DbCom.ExecuteNonQuery() > 0) successFullInserts++;
                });

                if (successFullInserts == reservationDTO.UserIds.Count + reservationDTO.WorkzoneIds.Count + 1) success = true;

            } 
            catch(Exception e) {
                Debug.WriteLine(e.Message);
                success = false; 
            }
            finally { CloseCon(); }
            return success;
        }
    }
}