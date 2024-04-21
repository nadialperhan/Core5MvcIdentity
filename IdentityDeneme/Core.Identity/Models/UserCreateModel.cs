using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage ="Zorunlu")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage ="doğru format girin")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Zorunlu")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Parolalar Eşleşmiyor")]
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
    }
}
