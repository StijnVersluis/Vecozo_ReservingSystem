using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public interface IWorkzone
    {
        public int GetAvailableWorkspaces(int id, DateTime datetime);

        public bool HasReservations(int id);
    }
}
