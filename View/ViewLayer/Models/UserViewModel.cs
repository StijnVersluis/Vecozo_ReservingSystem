using BusinessLayer;

namespace ViewLayer.Models
{
    public class UserViewModel
    {
        public int Id { private set; get; }
        public string Name { private set; get; }
        public Role Role { get; private set; }
        public bool IsBhv { get; private set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            IsBhv = user.IsBhv;
        }
        public UserViewModel(User user, Role role)
        {
            Id = user.Id;
            Name = user.Name;
            Role = role;
            IsBhv = user.IsBhv;
        }
    }
}
