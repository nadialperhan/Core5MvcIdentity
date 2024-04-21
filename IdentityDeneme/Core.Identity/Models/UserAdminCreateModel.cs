using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    public class UserAdminCreateModel
    {
        [Required(ErrorMessage ="Gerekli")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Gerekli")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Gerekli")]

        public string Gender { get; set; }
    }
}
