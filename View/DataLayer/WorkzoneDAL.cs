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
    public class WorkzoneDAL : SqlConnect, IWorkzoneContainer
    {
        private SqlDataReader reader;
        public WorkzoneDAL()
        {
            InitializeDB();
        }

        public WorkzoneDTO Get(int id)
        {
            OpenCon();
            WorkzoneDTO workzone = null;
            DbCom.CommandText = "SELECT * FROM Workzones WHERE Id = @id";
            DbCom.Parameters.AddWithValue("id", id);
            reader = DbCom.ExecuteReader();

            while (reader.FieldCount > 0 && reader.Read())
            {
                workzone = new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"]);
            }

            CloseCon();
            return workzone;
        }

        public List<WorkzoneDTO> GetAll()
        {
            OpenCon();
            List<WorkzoneDTO> workzones = new List<WorkzoneDTO>();
            DbCom.CommandText = "SELECT * FROM Workzones";
            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                workzones.Add(new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"]));
            }

            CloseCon();
            return workzones;
        }

        public List<WorkzoneDTO> GetAllFromFloor(int id)
        {
            OpenCon();
            List<WorkzoneDTO> workzones = new List<WorkzoneDTO>();
            DbCom.CommandText = "SELECT * FROM Workzones WHERE Floor = @id";
            DbCom.Parameters.AddWithValue("id", id);
            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                workzones.Add(new WorkzoneDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Workspaces"], (int)reader["Floor"]));
            }

            CloseCon();
            return workzones;
        }
    }
}
