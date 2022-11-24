using IntefaceLayer.DTO;
using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class FloorDAL : SqlConnect, IFloorContainer
    {
        private SqlDataReader reader;
        public FloorDAL() { InitializeDB(); }
        public List<FloorDTO> GetAll()
        {
            var FloorList = new List<FloorDTO>();
            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT Id, Name FROM Floor";
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    FloorList.Add(new FloorDTO((int)reader["Id"], (string)reader["Name"]));
                }
            } catch (Exception e) { }
            finally
            {
                CloseCon();
            }
            return FloorList;
        }
    }
}
