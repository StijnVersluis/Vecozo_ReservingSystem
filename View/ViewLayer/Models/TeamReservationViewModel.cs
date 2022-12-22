using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;

namespace ViewLayer.Models
{
    public class TeamReservationViewModel
    {
        public int Id { get; set; }
        public DateTime TimeArriving { get; set; }
        public DateTime TimeLeaving { get; set; }

        public Team Team { get; set; }
        public List<Workzone> Workzones { get; set; } = new();

        public TeamReservationViewModel(TeamReservation reservation)
        {
            Id = reservation.Id;
            TimeArriving = reservation.TimeArriving;
            TimeLeaving = reservation.TimeLeaving;

            var teamContainer = new TeamContainer(new TeamDAL());
            var workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());

            Team = teamContainer.GetTeam(reservation.TeamId);
            reservation.WorkzoneIds.ForEach(x => Workzones.Add(workzoneContainer.GetById(x)));
        }
    }
}
