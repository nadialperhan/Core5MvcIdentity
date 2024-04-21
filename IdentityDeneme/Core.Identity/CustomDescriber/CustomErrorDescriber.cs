using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.CustomDescriber
{
    public class CustomErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code= "PasswordTooShort",
                Description=$"Parola en az {length} karakter olmalıdır."
            };
            
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Parola en az bir alfanumerik karakter içermelidir."
            };
        }
        public override IdentityError DuplicateUserName(string username)
        {
            return new()
            {
                Code = "DuplicateUserName",
                Description = $"{username} sistemde kayıtlıdır."
            };
        }
    }
}
