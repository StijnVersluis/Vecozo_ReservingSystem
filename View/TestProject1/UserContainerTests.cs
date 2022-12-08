using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UserContainerTests
    {
       [TestMethod]
        public void GetAll()
        {
            // Arrange
            UserContainer UserContainer = new UserContainer(new UserContainerMock());
            UserContainerMock UserContainerMock = new UserContainerMock();
            //Act
            var count = UserContainer.GetAll();
            //Assert
            Assert.AreEqual(count.Count, UserContainerMock.GetAll().Count);
            for (int i = 0; i < count.Count; i++)
            {
                Assert.AreEqual(count[i].Name, UserContainerMock.users[i].Name);
                Assert.AreEqual(count[i].Id, UserContainerMock.users[i].Id);
                Assert.AreEqual(count[i].Role, UserContainerMock.users[i].Role);    
            }

        }
        [TestMethod]
        public void GetUserById()
        {
            // Arrange
            UserContainer UserContainer = new UserContainer(new UserContainerMock());
            UserContainerMock UserContainerMock = new UserContainerMock();
            //Act
            var user = UserContainer.GetUserById(5);
            //Assert
            Assert.AreEqual(user.Name, UserContainerMock.users[0].Name);
            Assert.AreEqual(user.Id, UserContainerMock.users[0].Id);
            Assert.AreEqual(user.Role, UserContainerMock.users[0].Role);    

        }
        [TestMethod]
        public void AttemptLogin()
        {
            throw new NotImplementedException();    
            //// Arrange
            //UserContainer UserContainer = new UserContainer(new UserContainerMock());
            //UserContainerMock UserContainerMock = new UserContainerMock();
            ////Act
            //var user = UserContainer.AttemptLogin("Tim@gmail.com","12345");
            ////Assert
            //Assert.AreEqual(user, UserContainerMock._email+UserContainerMock._password);
          



        }


    }
}
