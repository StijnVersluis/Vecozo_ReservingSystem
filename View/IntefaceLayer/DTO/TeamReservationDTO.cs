using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer.DTO
{
    public class TeamReservationDTO
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public DateTime TimeArriving { get; set; }
        public DateTime TimeLeaving { get; set; }
        public List<int> WorkzoneIds { get; set; }
        public List<int> UserIds { get; set; }

        public TeamReservationDTO(int id, int teamId, DateTime timeArriving, DateTime timeLeaving, List<int> workzoneIds, List<int> userIds)
        {
            Id = id;
            TeamId = teamId;
            TimeArriving = timeArriving;
            TimeLeaving = timeLeaving;
            WorkzoneIds = workzoneIds;
            UserIds = userIds;
        }
        public TeamReservationDTO(int teamId, DateTime timeArriving, DateTime timeLeaving, List<int> workzoneIds, List<int> userIds)
        {
            TeamId = teamId;
            TimeArriving = timeArriving;
            TimeLeaving = timeLeaving;
            WorkzoneIds = workzoneIds;
            UserIds = userIds;
        }
        public TeamReservationDTO(int id, int teamId, DateTime timeArriving, DateTime timeLeaving, List<int> workzoneIds)
        {
            Id = id;
            TeamId = teamId;
            TimeArriving = timeArriving;
            TimeLeaving = timeLeaving;
            WorkzoneIds = workzoneIds;
        }
        public TeamReservationDTO() {
            WorkzoneIds = new();
            UserIds = new();
        }
    }
}
