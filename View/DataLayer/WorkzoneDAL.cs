using IntefaceLayer;
using IntefaceLayer.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace DataLayer
{
    public class WorkzoneDAL : SqlConnect, IWorkzoneContainer
    {
        private SqlDataReader reader;
        public WorkzoneDAL()
        {
            InitializeDB();
        }

        public WorkzoneDTO GetById(int id)
        {
            WorkzoneDTO workzone = null;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Workzones WHERE Id = @Id";
                DbCom.Parameters.AddWithValue("@Id", id);
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    workzone = new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"], (bool)reader["TeamOnly"]);
                }
            }
            finally
            {
                CloseCon();
            }

            return workzone;
        }

        // Notice: Not returning the correct amount of workspaces.
        public WorkzoneDTO GetByDateAndId(int id, string date)
        {
            return GetAvailableWorkzones(date).Where(x => x.Id == id).FirstOrDefault();
        }

        // Notice: Not returning the correct amount of workspaces.
        public List<WorkzoneDTO> GetAll()
        {
            List<WorkzoneDTO> workzones = new List<WorkzoneDTO>();

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Workzones";
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    workzones.Add(new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"], (bool)reader["TeamOnly"]));
                }
            } finally
            {
                CloseCon();
            }

            return workzones;
        }

        private bool WorkzoneHasReservations(int id, string date = "")
        {
            bool result = false;
            string query = string.Empty;
            
            if (string.IsNullOrEmpty(date))
            {
                query = 
                    "SELECT a.* FROM Workzones a " +
                    "INNER JOIN Reservations b " +
                    "ON b.Workzone_Id = a.Id " +
                    "WHERE a.Id = @id";
            } else
            {
                query =
                    "SELECT * FROM Workzones a " +
                    "WHERE a.Id = @id " +
                    "AND EXISTS " +
                    "(SELECT * FROM Reservations b " +
                    "WHERE b.Workzone_Id = a.Id AND " +
                    "(@date BETWEEN b.DateTime_Arriving AND b.DateTime_Leaving) OR " +
                    "(b.DateTime_Arriving >= @date AND @date <= b.DateTime_Leaving))";
            }

            try
            {
                if (DbCom.Connection.State == ConnectionState.Closed)
                {
                    OpenCon();
                }

                DbCom.CommandText = query;
                DbCom.Parameters.AddWithValue("id", id);

                if (!string.IsNullOrEmpty(date))
                {
                    DbCom.Parameters.AddWithValue("date", date);
                }


                reader = DbCom.ExecuteReader();
                DbCom.Parameters.Clear();

                result = reader.HasRows ? true : false;
            } finally
            {
                if (DbCom.Connection.State == ConnectionState.Open)
                {
                    CloseCon();
                }
            }

            return result;
        }

        private List<WorkzoneDTO> GetWorkzones(string date, int floorId = 0)
        {
            List<WorkzoneDTO> workzones = new List<WorkzoneDTO>();

            try
            {
                if (DbCom.Connection.State == ConnectionState.Closed)
                {
                    OpenCon();
                }

                string clause = floorId > 0 ? "WHERE a.Floor = @floor " : " ";
                DbCom.CommandText =
                    "SELECT a.Id, a.Name, a.PositionX, a.PositionY, a.Floor, a.TeamOnly, COUNT(a.Id) AS 'Workspaces' " +
                    "FROM Workzones a " +
                    "LEFT JOIN Reservations b " +
                    "ON b.Workzone_Id = a.Id " +
                    $"AND (DateTime_Arriving >= @date AND @date <= b.DateTime_Leaving) {clause} " +
                    "GROUP BY a.Id, a.Name, a.PositionX, a.PositionY, a.Floor, a.TeamOnly";

                if (floorId > 0)
                {
                    DbCom.Parameters.AddWithValue("floor", floorId);
                }

                DbCom.Parameters.AddWithValue("date", date);
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    workzones.Add(new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"], (bool)reader["TeamOnly"]));
                }
            } finally
            {
                if (DbCom.Connection.State == ConnectionState.Open)
                {
                    CloseCon();
                }
            }

            return workzones;
        }

        public List<WorkzoneDTO> GetAvailableWorkzones(string date, int id = 0)
        {
            List<WorkzoneDTO> workzones = new List<WorkzoneDTO>();

            GetWorkzones(date, id).ForEach(x =>
            {
                var workzone = GetById(x.Id);
                if (workzone != null)
                {
                    int max = workzone.Workspaces;
                    int min = 0;

                    if (workzone == null) return;
                    if (workzone.Id != x.Id) return;

                    // Check if the workzone is not an "ST" because it always has 1 workzone
                    // Check if the workzone has reservations.
                    if (!workzone.Name.Contains("ST") && WorkzoneHasReservations(workzone.Id))
                    {
                        workzone.Workspaces = workzone.Workspaces > min ? workzone.Workspaces - x.Workspaces : min;
                    }

                    // Increment seats if workzone has no reservation
                    if (!WorkzoneHasReservations(workzone.Id, date))
                    {
                        //workzone.Workspaces = max;
                        workzone.Workspaces = workzone.Workspaces < max ? workzone.Workspaces + 1 : max;
                    }

                    workzones.Add(workzone);
                }
        });

            return workzones;
        }

        public List<WorkzoneDTO> GetAllFromFloor(int id, string date)
        {
            return GetAvailableWorkzones(date, id);
        }
    }
}
