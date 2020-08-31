using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;


namespace IRO_UNMO.WebAPI.Database
{
    public partial class IRO_UNMO_Context : DbContext
    {

        public IRO_UNMO_Context(DbContextOptions<IRO_UNMO_Context> options)
            : base(options)
        {
        }
        public IRO_UNMO_Context()
        {
        }

        public DbSet<Country> Countries { get; set; }
    }
}
