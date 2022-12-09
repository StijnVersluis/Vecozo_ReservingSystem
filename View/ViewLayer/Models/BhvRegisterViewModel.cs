using BusinessLayer;
using System.ComponentModel;

namespace ViewLayer.Models
{
    public class BhvRegisterViewModel
    {
        public int Id { private set; get; }
        [DisplayName("Gebruiker")]
        public string Name { private set; get; }
        [DisplayName("Bhver")]
        public bool IsBhv { get; private set; }
        [DisplayName("Aanwezig")]
        public bool IsAvailable { get; private set; }

        public BhvRegisterViewModel(User user, bool isAvailable)
        {
            Id = user.Id;
            Name = user.Name;
            IsBhv = user.IsBhv;
            IsAvailable = isAvailable;
        }
    }
}
