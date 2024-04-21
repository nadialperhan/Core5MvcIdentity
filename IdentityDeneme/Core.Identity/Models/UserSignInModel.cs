using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage ="Kullanıcı adı gerekli")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Parola gerekli")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
