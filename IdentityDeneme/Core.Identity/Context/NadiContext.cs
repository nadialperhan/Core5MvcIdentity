using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Context
{
    public class NadiContext:IdentityDbContext<AppUser,AppRole,int>
    {
        public NadiContext(DbContextOptions<NadiContext> options):base(options)
        {

        }
    }
}
