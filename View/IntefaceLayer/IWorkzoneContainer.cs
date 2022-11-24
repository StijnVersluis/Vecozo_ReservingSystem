using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public interface IWorkzoneContainer
    {
        public List<WorkzoneDTO> GetAll();
        public List<WorkzoneDTO> GetAllFromFloor(int id);
    }
}
