using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    public class RoleCreateModel
    {
        [Required(ErrorMessage ="Gerekli")]
        public string Name { get; set; }
    }
}
