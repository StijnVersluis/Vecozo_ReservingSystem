using BusinessLayer;
using System;
using System.Collections.Generic;

namespace ViewLayer.Models
{
    public class OutsourcingReservationViewModel
    {
        public List<ReservationViewModel> AllUserReservations { get; set; }
        public List<WorkzoneViewModel> AllWorkzones { get; set; }
        public List<TeamViewModel> TeamsOfUser { get; set; }
        public UserViewModel LoggedInUser { get; set; }
        public DateTime? dateTime_Planned_Start { get; set; }
        public DateTime? dateTime_Planned_Leaving { get; set; }
        public List<User>? SelectedUsers { get; set; } 
        public int? SelectedFloor { get; set; }
        public bool IsTeam { get; set; }

        public OutsourcingReservationViewModel(List<ReservationViewModel> allUserReservations, List<WorkzoneViewModel> allWorkzones, List<TeamViewModel> teamsOfUser, UserViewModel loggedInUser)
        {
            AllUserReservations = allUserReservations;
            AllWorkzones = allWorkzones;
            TeamsOfUser = teamsOfUser;
            LoggedInUser = loggedInUser;
        }

        public OutsourcingReservationViewModel()
        {

        }
    }
}
