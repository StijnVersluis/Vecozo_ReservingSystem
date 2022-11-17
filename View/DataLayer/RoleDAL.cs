using IntefaceLayer.DTO;
using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RoleDAL : SqlConnect, IRoleContainer
    {
        SqlDataReader reader;
        public RoleDAL()
        {
            InitializeDB();
        }

        public RoleDTO GetRole(int roleId)
        {
            RoleDTO role = null;

            try
            {
                OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText = "SELECT * FROM Roles WHERE Id = @id";
                DbCom.Parameters.AddWithValue("id", roleId);
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    role = new RoleDTO((int)reader["Id"], (string)reader["Name"]);
                }
            }
            finally
            {
                CloseCon();
            }

            return role;
        }

        public List<RoleDTO> GetRoles()
        {
            List<RoleDTO> roles = new();

            try
            {
                OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText = "SELECT * FROM Roles";
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new RoleDTO((int)reader["Id"], (string)reader["Name"]));
                }
            }
            finally
            {
                CloseCon();
            }

            return roles;
        }
    }
}
