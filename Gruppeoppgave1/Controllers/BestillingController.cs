using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillingController : ControllerBase
    {
        private readonly NORWAYContext _db;

        public BestillingController(NORWAYContext db)
        {
            _db = db;
        }

        public async Task<bool> LagBestilling(int avgangId, int antall)
        {
            try
            {
                var nyBestillingRad = new Bestilling();

                var AvgangValg = _db.Avganger.Find(avgangId);

                if(AvgangValg != null)
                {
                    nyBestillingRad.Avgang = AvgangValg;
                    nyBestillingRad.Antall = antall;

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

        public async Task<List<Bestilling>> HentAlleBestillinger()
        {
            try
            {
                List<Bestilling> alleBestillinger = await _db.Bestillinger.Select(b => new Bestilling
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
