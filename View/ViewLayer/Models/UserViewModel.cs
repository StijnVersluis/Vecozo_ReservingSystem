using BusinessLayer;

namespace ViewLayer.Models
{
    public class UserViewModel
    {
        public int Id { private set; get; }
        public string Name { private set; get; }
        public int Role { private set; get; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Role = user.Role;
        }

        //public UserViewModel()
        //{
        //}
    }
}
