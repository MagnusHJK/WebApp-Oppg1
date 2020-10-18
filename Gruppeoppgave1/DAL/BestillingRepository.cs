using Castle.Core.Internal;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public class BestillingRepository : IBestillingRepository
    {
        private readonly NORWAYContext _db;

        public BestillingRepository(NORWAYContext db)
        {
            _db = db;
        }

        public async Task<bool> LagBestilling(int avgangId, int antall, int brukerId)
        {
            try
            {
                var nyBestillingRad = new Bestillinger();

                Avganger ValgtAvgang = await _db.Avganger.FindAsync(avgangId);
                Brukere ValgtBruker = await _db.Brukere.FindAsync(brukerId);

                if (ValgtAvgang != null)
                {
                    nyBestillingRad.Avgang = ValgtAvgang;
                    nyBestillingRad.Antall = antall;
                    nyBestillingRad.Bruker = ValgtBruker;

                    _db.Bestillinger.Add(nyBestillingRad);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EndreBestilling(int bestillingId, int nyAvgangId, int nyttAntall)
        {
            try
            {
                Bestillinger endretBestilling = await _db.Bestillinger.FirstOrDefaultAsync(b => b.Id == bestillingId);

                if (endretBestilling != null)
                {
                    Avganger nyAvgang = await _db.Avganger.FirstOrDefaultAsync(a => a.Id == nyAvgangId);

                    if(nyAvgang != null)
                    {
                        endretBestilling.Avgang = nyAvgang;
                        endretBestilling.Antall = nyttAntall;
                        await _db.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;

            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SlettBestilling(int bestillingId)
        {
            try
            {
                Bestillinger slettetBestilling = await _db.Bestillinger.FirstOrDefaultAsync(b => b.Id == bestillingId);
                _db.Bestillinger.Remove(slettetBestilling);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Henter bestillinger for gitt bruker
        public async Task<List<Bestillinger>> HentBestillinger(int brukerId)
        {
            try
            {
                List<Bestillinger> bestillinger = await _db.Bestillinger.Where(b => b.Bruker.Id == brukerId).ToListAsync();
                return bestillinger;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Bestillinger>> HentAlleBestillinger()
        {
            try
            {
                List<Bestillinger> alleBestillinger = await _db.Bestillinger.Select(b => new Bestillinger
                {
                    Id = b.Id,
                    Avgang = b.Avgang,
                    Antall = b.Antall

                }).ToListAsync();

                return alleBestillinger;
            }
            catch
            {
                return null;
            }
        }
    }
}

