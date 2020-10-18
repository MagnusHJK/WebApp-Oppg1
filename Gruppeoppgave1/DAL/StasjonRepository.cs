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
        public async Task<List<Stasjoner>> HentAlleStasjoner()
        {
            try
            {
                List<Stasjoner> alleStasjoner = await _db.Stasjoner.ToListAsync();
                return alleStasjoner;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Stasjoner> HentEnStasjon(int id)
        {
            try
            {
                Stasjoner enStasjon = await _db.Stasjoner.FindAsync(id);
                await _db.SaveChangesAsync();
                return enStasjon;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> LagStasjon(Stasjon stasjon)
        {
            try
            {
                Stasjoner nyStasjon = new Stasjoner
                {
                    Navn = stasjon.Navn
                };
                await _db.Stasjoner.AddAsync(nyStasjon);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EndreStasjon(Stasjon stasjon)
        {
            try
            {
                Stasjoner endreStasjon = await _db.Stasjoner.FirstOrDefaultAsync(s => s.Id == stasjon.Id);
                if(endreStasjon != null)
                {
                    endreStasjon.Navn = stasjon.Navn;
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SlettStasjon(int id)
        {
            try
            {
                Stasjoner slettStasjon = await _db.Stasjoner.FindAsync(id);
                _db.Stasjoner.Remove(slettStasjon);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

