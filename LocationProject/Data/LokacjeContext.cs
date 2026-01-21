using System.Collections.Generic;
using LocationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocationProject.Data
{
    public class LokacjeContext : IdentityDbContext<IdentityUser>
    {
        public LokacjeContext(DbContextOptions<LokacjeContext> options)
            : base(options)
        {
        }
        public DbSet<Lokacja> Lokacje { get; set; }
    }
}
