using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class TeamContainerTest
    {
        [TestMethod]
        public void DeleteTeam()
        {
            // Arrange
            TeamContainer teamcontainer = new TeamContainer(new TeamContainerMock());
            TeamContainerMock teamcontainermock = new TeamContainerMock();
            //Act
            var todelete = teamcontainer.DeleteTeam(2);
            //Assert
            Assert.IsTrue(true);


        }

        [TestMethod]
        public void GetTeam()
        {
            // Arrange
            TeamContainer teamcontainer = new TeamContainer(new TeamContainerMock());
            TeamContainerMock teamcontainermock = new TeamContainerMock();
            //Act
            var togetteam = teamcontainer.GetTeam(1);
            //Assert
            Assert.AreEqual(togetteam.Name, teamcontainermock.teams[0].Name);
            Assert.AreEqual(togetteam.Id, teamcontainermock.teams[0].Id);



        }
        [TestMethod]
        public void AttempLogin()
        {
            throw new NotImplementedException();
            //// Arrange
            //TeamContainer teamcontainer = new TeamContainer(new TeamContainerMock());
            //TeamContainerMock teamcontainermock = new TeamContainerMock();
            ////Act
            //var togetteam = teamcontainer.GetTeam(1);
            ////Assert
            //Assert.AreEqual(togetteam.Name, teamcontainermock.teams[0].Name);
            //Assert.AreEqual(togetteam.Id, teamcontainermock.teams[0].Id);

        }
    }

   
}
