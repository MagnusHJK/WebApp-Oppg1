using Gruppeoppgave1.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    [ExcludeFromCodeCoverage]
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<NORWAYContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                string[] stasjonNavn = 
                { 
                    "Oslo", "Skien", "Porsgrunn", "Arendal", "Kristiansand", 
                    "Stavanger", "Haugesund", "Seljestad", "Bergen", "Lærdal", 
                    "Sogndal", "Fagernes", "Lillehammer", "Trysil", "Førde", "Stryn" 
                };

                List<Stasjoner> stasjoner = new List<Stasjoner>();

                for(int i = 0; i < stasjonNavn.Length; i++)
                {
                    stasjoner.Add(new Stasjoner { Navn = stasjonNavn[i] });
                }

                context.Stasjoner.AddRange(stasjoner);

                var bruker = new Brukere();
                bruker.Brukernavn = "Admin";
                string passord = "Admin-123";
                byte[] salt = BrukerRepository.LagSalt();
                byte[] hash = BrukerRepository.LagHash(passord, salt);
                bruker.Passord = hash;
                bruker.Salt = salt;
                context.Brukere.Add(bruker);

               context.SaveChanges();
            }
        }
    }
}
