﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignLanguage.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SignLanguage.Infrastracture.Data.Identity
{
    public class AppIdentityDbContext :IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
        {
            
        }

        
    }
}
