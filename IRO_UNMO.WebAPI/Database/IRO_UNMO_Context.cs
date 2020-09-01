using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IRO_UNMO.WebAPI.Database
{
    public partial class IRO_UNMO_Context : IdentityDbContext<ApplicationUser>
    {

        public IRO_UNMO_Context(DbContextOptions<IRO_UNMO_Context> options)
            : base(options)
        {
        }
        public IRO_UNMO_Context()
        {
        }

        public DbSet<Country> Country { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<University> University { get; set; }
    }
}
