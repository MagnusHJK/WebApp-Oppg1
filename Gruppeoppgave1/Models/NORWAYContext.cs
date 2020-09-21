using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gruppeoppgave1.Models
{
    public class Stasjoner
    {
        [Key]
        public int Id { get; set; }
        public string Navn { get; set; }
    }

    public class Avganger
    {
        [Key]
        public int Id { get; set; }
        
        virtual public Stasjon StasjonFra { get; set; }

        virtual public Stasjon StasjonTil { get; set; }

        public string Dato { get; set; }

        public string Tidspunkt { get; set; }

        public string Pris { get; set; }
    }

    public class Bestillinger
    {
        [Key]
        public int Id { get; set; }

        virtual public Avgang Avgang { get; set; }
    }

    public class NORWAYContext : DbContext
    {
        public NORWAYContext (DbContextOptions<NORWAYContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Stasjon> Stasjoner { get; set; }
        public DbSet<Bestilling> Bestillinger { get; set; }
        public DbSet<Avgang> Avganger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
