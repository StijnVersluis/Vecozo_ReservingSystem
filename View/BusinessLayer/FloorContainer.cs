using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class FloorContainer
    {
        private IFloorContainer iFloorContainer;
        public FloorContainer(IFloorContainer iFloorContainer)
        {
            this.iFloorContainer = iFloorContainer;
        }

        public List<Floor> GetAll()
        {
            return iFloorContainer.GetAll().ConvertAll(floorDTO => new Floor(floorDTO));
        }
    }
}
