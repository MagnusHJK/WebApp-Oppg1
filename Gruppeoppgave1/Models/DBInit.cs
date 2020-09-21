using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<NORWAYContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var stasjon1 = new Stasjon { Navn = "Oslo" };
                var stasjon2 = new Stasjon { Navn = "Fredrikstad" };
                var stasjon3 = new Stasjon { Navn = "Kristiansand" };
                var stasjon4 = new Stasjon { Navn = "Trondheim" };
                var stasjon5 = new Stasjon { Navn = "Bergen" };
                var stasjon6 = new Stasjon { Navn = "Stavanger" };
                var stasjon7 = new Stasjon { Navn = "Bodø" };
                var stasjon8 = new Stasjon { Navn = "Tromsø" };

                context.Stasjoner.Add(stasjon1);
                context.Stasjoner.Add(stasjon2);
                context.Stasjoner.Add(stasjon3);
                context.Stasjoner.Add(stasjon4);
                context.Stasjoner.Add(stasjon5);
                context.Stasjoner.Add(stasjon6);
                context.Stasjoner.Add(stasjon7);
                context.Stasjoner.Add(stasjon8);

                var avgang1 = new Avgang { Dato = "09/19/2020", StasjonFra = stasjon1, StasjonTil = stasjon2, Tidspunkt = "0:00", Pris = 200 };

                var bestilling1 = new Bestilling { Avgang = avgang1, Antall = 1 };
                context.Bestillinger.Add(bestilling1);

                context.SaveChanges();
            }
        }
    }
}
