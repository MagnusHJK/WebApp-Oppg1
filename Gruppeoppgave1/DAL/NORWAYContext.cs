using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gruppeoppgave1.DAL
{
    [ExcludeFromCodeCoverage]
    public class Stasjoner
    {
        [Key]
        public int Id { get; set; }
        public string Navn { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Avganger
    {
        [Key]
        public int Id { get; set; }
        
        virtual public Stasjoner StasjonFra { get; set; }

        virtual public Stasjoner StasjonTil { get; set; }

        public DateTime Dato { get; set; }

        public int Pris { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Bestillinger
    {
        [Key]
        public int Id { get; set; }

        virtual public Avganger Avgang { get; set; }

        public int Antall { get; set; }

        virtual public Brukere Bruker { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Brukere
    {
        [Key]
        public int Id { get; set; }

        public string Brukernavn { get; set; }

        public byte[] Passord { get; set; }

        public byte[] Salt { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class NORWAYContext : DbContext
    {
        public NORWAYContext (DbContextOptions<NORWAYContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Stasjoner> Stasjoner { get; set; }
        public DbSet<Bestillinger> Bestillinger { get; set; }
        public DbSet<Avganger> Avganger { get; set; }
        public DbSet<Brukere> Brukere { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
