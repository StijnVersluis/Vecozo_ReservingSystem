using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecozoTests
{
    public class WorkzoneSTUB : IWorkzoneContainer
    {
        public List<WorkzoneDTO> Workzones;
        public WorkzoneSTUB()
        {
            //int id, string name, int workspaces, int floor, bool teamOnly, int xpos, int ypos
            Workzones = new List<WorkzoneDTO>()
            {
                new WorkzoneDTO(1, "WB-01", 4, 1, false, 0, 0),
                new WorkzoneDTO(2, "WB-02", 4, 1, false, 0, 0),
                new WorkzoneDTO(3, "WB-03", 3, 1, false, 0, 0),
                new WorkzoneDTO(4, "WB-04", 3, 1, false, 0, 0),
                new WorkzoneDTO(5, "WB-05", 8, 1, true, 0, 0),
                new WorkzoneDTO(6, "WB-06", 8, 1, true, 0, 0),
                new WorkzoneDTO(7, "WB-01", 4, 2, false, 0, 0),
                new WorkzoneDTO(8, "WB-02", 2, 2, false, 0, 0),
                new WorkzoneDTO(9, "WB-03", 2, 2, false, 0, 0),
                new WorkzoneDTO(10, "WB-04", 5, 2, false, 0, 0),
                new WorkzoneDTO(11, "WB-05", 5, 2, false, 0, 0),
            };
            
        }

        public bool DeleteWorkzone(int id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(WorkzoneDTO workzoneDTO)
        {
            throw new NotImplementedException();
        }

        public List<WorkzoneDTO> GetAll()
        {
            return Workzones;
        }

        public List<WorkzoneDTO> GetAllFromFloor(int id)
        {
            return Workzones.Where(workzone => workzone.Floor == id).ToList();
        }

        public List<WorkzoneDTO> GetAllFromFloorWithDate(int id, string date)
        {
            throw new NotImplementedException();
        }

        public WorkzoneDTO GetByDateAndId(int id, string date)
        {
            throw new NotImplementedException();
        }

        public WorkzoneDTO GetById(int id)
        {
            return Workzones.Where(workzone => workzone.Id == id).First();
        }
    }
}
