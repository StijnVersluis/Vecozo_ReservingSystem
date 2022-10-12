using IntefaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class WorkzoneContainer
    {
        private IWorkzoneContainer workzoneContainer;
        public WorkzoneContainer(IWorkzoneContainer workzoneContainer)
        {
            this.workzoneContainer = workzoneContainer;
        }

        public List<Workzone> GetAll()
        {
            return workzoneContainer.GetAll().ConvertAll(x => new Workzone(x));
        }
        public List<Workzone> GetAllFromFloor(int id)
        {
            return workzoneContainer.GetAllFromFloor(id).ConvertAll(x => new Workzone(x));
        }
    }
}
