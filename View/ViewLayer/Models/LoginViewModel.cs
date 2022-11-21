using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Het invoeren van je e-mail is verplicht!")]
        [DisplayName("E-mail")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Het invoeren van je wachtwoord is verplicht!")]
        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
