using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;

namespace IntefaceLayer
{
    public interface IWorkzoneContainer
    {
        public WorkzoneDTO GetById(int id);
        public WorkzoneDTO GetByDateAndId(int id, string date);
        public List<WorkzoneDTO> GetAll();
        public List<WorkzoneDTO> GetAllFromFloor(int id, string date);
    }
}
