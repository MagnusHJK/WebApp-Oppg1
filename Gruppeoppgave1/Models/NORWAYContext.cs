using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gruppeoppgave1.Models
{
    public class NORWAYContext : DbContext
    {
        public NORWAYContext (DbContextOptions<NORWAYContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Stasjon> Stasjoner { get; set; }
        public DbSet<Bestilling> Bestillinger { get; set; }
        public DbSet<Avgang> Avganger { get; set; }
    }
}
