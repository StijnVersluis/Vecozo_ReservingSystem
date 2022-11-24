using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class WorkzoneContainer
    {
        private IWorkzoneContainer workzoneContainer;
        public WorkzoneContainer(IWorkzoneContainer workzoneContainer)
        {
            this.workzoneContainer = workzoneContainer;
        }

        public Workzone GetById(int id)
        {
            Workzone output = null;
            WorkzoneDTO result = workzoneContainer.GetById(id);

            if (result != null)
            {
                output = new Workzone(result);
            }

            return output;
        }

        public Workzone GetByDateAndId(int id, string date)
        {
            Workzone output = null;
            WorkzoneDTO result = workzoneContainer.GetByDateAndId(id, date);

            if (result != null)
            {
                output = new Workzone(result);
            }

            return output;
        }

        public List<Workzone> GetAll()
        {
            return workzoneContainer.GetAll().ConvertAll(x => new Workzone(x));
        }
        public List<Workzone> GetAllFromFloor(int id)
        {
            return workzoneContainer.GetAllFromFloor(id).ConvertAll(x => new Workzone(x));
        }
        public List<Workzone> GetAllFromFloorWithDate(int id, string date)
        {
            return workzoneContainer.GetAllFromFloorWithDate(id, date).ConvertAll(x => new Workzone(x));
        }
        public bool Updateworkspace(Workzone workzone)

        {
            WorkzoneDTO workzoneDTO = new WorkzoneDTO();
            workzoneDTO.Workspaces = workzone.Workspaces;
            workzoneDTO.Id = workzone.Id;   
            return workzoneContainer.Updateworkspace(workzoneDTO);
        }
    }
}
