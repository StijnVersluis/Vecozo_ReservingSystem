using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class TeamContainer
    {
        private ITeam iTeam;

        public TeamContainer(ITeam iteam)
        {
            iTeam = iteam;
        }

        public bool DeleteTeam(int teamId)
        {
            return iTeam.DeleteTeam(teamId);
        }

        public Team GetTeam(int id)
        {
            return new Team(iTeam.GetTeam(id));
        }
        public Team GetTeam(int id)
        {
            return new Team(iTeam.GetTeam(id));
        }
        public List<Team> GetTeamsOfUser(int userId)
        {
            return iTeam.GetTeamsOfUser(userId).ConvertAll(teamdto => new Team(teamdto));
        }
        public bool CreateTeam(string name, List<int> userids)
        {
            return iTeam.CreateTeam(name, userids);
        }

        public bool EditTeam(Team team, List<int> userids)
        {
            return iTeam.EditTeam(team.ToDTO(), userids);
        }

        public bool IsTeamAdmin(int teamId, int userId)
        {
            return iTeam.IsTeamAdmin(teamId, userId);
        }

        public bool LeaveTeam(int teamId, int userId)
        {
            if (IsTeamAdmin(teamId, userId))
            {
                return false;
            }

            return iTeam.LeaveTeam(teamId, userId);
        }

        public User GetTeamAdmin(int teamId)
        {
            return new User(iTeam.GetTeamAdmin(teamId));
        }

        public List<string> CheckEditRules(string name, int adminId, List<int> membersIds)
        {
            List<string> messages = new();

            messages.AddRange(CheckGeneralRules(name, adminId, membersIds));

            // Check of de admin zich bevindt in de member lijst.
            if (membersIds.Where(x => x == adminId).FirstOrDefault() == 0)
            {
                messages.Add("Je moet deel uitmaken van de groep.");
            }

            return messages;
        }

        public List<string> CheckGeneralRules(string name, int adminId, List<int> membersIds)
        {
            List<string> messages = new();

            // Check if a team name has been given.
            if (String.IsNullOrEmpty(name))
            {
                messages.Add("Het team moet een naam hebben!");
            }

            // Check of de team niet leeg is.
            if (membersIds.Count == 0)
            {
                messages.Add("Het team kan niet leeg zijn!");
            }

            // Check of er minimaal een werknemer in het team bevindt.
            if (membersIds.Count == 1 && membersIds.First() == adminId)
            {
                messages.Add("U moet minimaal een werknemer toevoegen aan het team!");
            }

            return messages;
        }

        public bool Check_Accessibility(string username)
        {

            return iTeam.Check_Accessibility(username);
        }
    }
}

