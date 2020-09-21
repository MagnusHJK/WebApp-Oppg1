using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public class StasjonRepository : IStasjonRepository
    {
        private readonly NORWAYContext _db;

        public StasjonRepository(NORWAYContext db)
        {
            _db = db;
        }

        //Komplett array av stasjoner
        public async Task<List<Stasjon>> HentAlleStasjoner()
        {
            try
            {
                List<Stasjon> alleStasjoner = await _db.Stasjoner.ToListAsync();
                return alleStasjoner;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Stasjon> HentEnStasjon(int id)
        {
            try
            {
                Stasjon enStasjon = await _db.Stasjoner.FindAsync(id);
                await _db.SaveChangesAsync();
                return enStasjon;
            }
            catch
            {
                return null;
            }
        }



        public bool Test()
        {
            return true;
        }
    }
}

