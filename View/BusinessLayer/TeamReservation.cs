using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TeamReservation
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public DateTime TimeArriving { get; set; }
        public DateTime TimeLeaving { get; set; }
        public List<int> WorkzoneIds { get; set; }
        public List<int> UserIds { get; set; }

        public TeamReservation(int id, int team, DateTime timeArriving, DateTime timeLeaving, List<int> workzoneIds, List<int> userIds)
        {
            Id = id;
            TeamId = team;
            TimeArriving = timeArriving;
            TimeLeaving = timeLeaving;
            WorkzoneIds = workzoneIds;
            UserIds = userIds;
        }
        public TeamReservation(int teamId, DateTime timeArriving, DateTime timeLeaving, List<int> workzoneIds, List<int> userIds)
        {
            TeamId = teamId;
            TimeArriving = timeArriving;
            TimeLeaving = timeLeaving;
            WorkzoneIds = workzoneIds;
            UserIds = userIds;
        }
        public TeamReservation(TeamReservationDTO teamReservationDTO)
        {
            TeamId = teamReservationDTO.TeamId;
            TimeArriving = teamReservationDTO.TimeArriving;
            TimeLeaving = teamReservationDTO.TimeLeaving;
            WorkzoneIds = teamReservationDTO.WorkzoneIds;
            UserIds = teamReservationDTO.UserIds;
        }
        public bool AddUser(User user)
        {
            try { UserIds.Add(user.Id); return true; }
            catch (Exception) { return false; }
        }
        public TeamReservationDTO ToDTO(bool WithId = true)
        {
            if (WithId)
            {
                return new TeamReservationDTO(Id, TeamId, TimeArriving, TimeLeaving, WorkzoneIds, UserIds);
            }
            else
            {
                return new TeamReservationDTO(TeamId, TimeArriving, TimeLeaving, WorkzoneIds, UserIds);
            }
        }
    }
}
