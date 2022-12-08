using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class UserContainerMock : IUser, IUserContainer
    {
       public string _email = "Tim@gmail.com";
         public string _password = "12345"; 
       
       public List<UserDTO> users = new List<UserDTO>();

        public UserContainerMock()
        {
            UserDTO user1= new UserDTO(5, "Tim", 1);
            UserDTO user2 = new UserDTO(6, "Jack", 0);
          
             users.Add(user1);  
             users.Add(user2);
         
           
             
        }

        public bool AttemptLogin(string email, string password)
        {
           if(_email==email&&_password==password)
            {
                return true;
            }
           return false;    
         
        }
        public UserDTO FindUserByEmail(string email)
        {
            throw new NotImplementedException();    

          
           
        }

        public List<UserDTO> GetAll()
        {
         return users;  
        }

        public List<UserDTO> GetFilteredUsers(string filterStr)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserById(int id)
        {
            if (id != 0)
            {
                var result = users.Find(x => x.Id == id);
                return result;
            }
            return null;
        }

        public bool IsLoggedIn()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
